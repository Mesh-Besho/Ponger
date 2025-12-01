import math
import pyray


EPS = 1e-6
MAX_BOUNCES = 16


class Circle:
    def __init__(self, center: pyray.Vector2, radius: float):
        self.center = center
        self.radius = radius


class Line:
    def __init__(self, a: pyray.Vector2, b: pyray.Vector2):
        self.a = a
        self.b = b


def clamp01(x: float) -> float:
    return 0.0 if x < 0.0 else (1.0 if x > 1.0 else x)


def clamp(x: float, a: float, b: float) -> float:
    return a if x < a else (b if x > b else x)


def reflect(v: pyray.Vector2, n: pyray.Vector2) -> pyray.Vector2:
    """Reflect vector v about normalized vector n."""
    dot = pyray.vector2_dot_product(v, n)
    return pyray.vector2_subtract(v, pyray.vector2_scale(n, 2.0 * dot))


def solve_quadratic_for_hit(p: pyray.Vector2, move: pyray.Vector2, E: pyray.Vector2, r: float):
    """Solve |(p + move * t) - E|^2 = r^2. Return (hit: bool, t_out: float)."""
    m = pyray.vector2_subtract(p, E)
    a = pyray.vector2_dot_product(move, move)
    b = 2 * pyray.vector2_dot_product(m, move)
    c = pyray.vector2_dot_product(m, m) - r * r

    if abs(a) < 1e-12:
        return False, 0.0

    disc = b * b - 4 * a * c
    if disc < 0:
        return False, 0.0

    sqrt_d = math.sqrt(disc)
    t0 = (-b - sqrt_d) / (2 * a)
    t1 = (-b + sqrt_d) / (2 * a)

    t_candidate = float('inf')
    if -1e-8 <= t0 <= 1.0 + 1e-8:
        t_candidate = t0
    if -1e-8 <= t1 <= 1.0 + 1e-8 and t1 < t_candidate:
        t_candidate = t1

    if t_candidate == float('inf'):
        return False, 0.0

    return True, clamp01(t_candidate)


def update_circle(circle: Circle, velocity: pyray.Vector2, walls: list[Line], dt: float):
    """Update circle position and velocity for one tick (continuous motion, exact collision)."""
    remaining = dt
    EPS_FACTOR = 1e-4
    eps = EPS_FACTOR * circle.radius

    for _ in range(MAX_BOUNCES):
        if remaining <= EPS:
            break

        move = pyray.vector2_scale(velocity, remaining)
        earliest_t = 1.0 + 1e-9
        hit_candidates = []

        for wall in walls:
            A, B = wall.a, wall.b
            w = pyray.vector2_subtract(B, A)
            w_len = pyray.vector2_length(w)
            if w_len < EPS:
                continue

            w_hat = pyray.vector2_scale(w, 1.0 / w_len)

            # 1) Infinite line (both sides)
            n_base = pyray.Vector2(-w_hat.y, w_hat.x)
            for side in (-1, 1):
                n = pyray.vector2_scale(n_base, side)
                dot_p = pyray.vector2_dot_product(pyray.vector2_subtract(circle.center, A), n)
                dot_vn = pyray.vector2_dot_product(move, n)
                if abs(dot_vn) < 1e-9:
                    continue

                t = (circle.radius - dot_p) / dot_vn
                if -EPS <= t <= 1.0 + EPS:
                    tt = clamp01(t)
                    C = pyray.vector2_add(circle.center, pyray.vector2_scale(move, tt))
                    s = pyray.vector2_dot_product(pyray.vector2_subtract(C, A), w_hat)
                    if -EPS <= s <= w_len + EPS:
                        closest = pyray.vector2_add(A, pyray.vector2_scale(w_hat, clamp(s, 0.0, w_len)))
                        diff = pyray.vector2_subtract(C, closest)
                        normal = pyray.vector2_normalize(diff)
                        if pyray.vector2_length(normal) < 1e-9:
                            continue

                        if tt < earliest_t - 1e-9:
                            earliest_t = tt
                            hit_candidates = [(normal, C)]
                        elif abs(tt - earliest_t) <= 1e-6:
                            hit_candidates.append((normal, C))

            # 2) Endpoints (caps)
            for E in (A, B):
                hit, t_cap = solve_quadratic_for_hit(circle.center, move, E, circle.radius)
                if not hit:
                    continue

                tt = clamp01(t_cap)
                C = pyray.vector2_add(circle.center, pyray.vector2_scale(move, tt))
                normal = pyray.vector2_normalize(pyray.vector2_subtract(C, E))
                if pyray.vector2_length(normal) < 1e-9:
                    continue

                if tt < earliest_t - 1e-9:
                    earliest_t = tt
                    hit_candidates = [(normal, C)]
                elif abs(tt - earliest_t) <= 1e-6:
                    hit_candidates.append((normal, C))

        # --- No collision ---
        if not hit_candidates or earliest_t > 1.0:
            circle.center = pyray.vector2_add(circle.center, move)
            break

        # --- Move to contact point ---
        circle.center = pyray.vector2_add(circle.center, pyray.vector2_scale(move, earliest_t))

        # --- Combine normals (for corners) ---
        combined = pyray.Vector2(0.0, 0.0)
        for n, _ in hit_candidates:
            combined = pyray.vector2_add(combined, n)
        combined = pyray.vector2_normalize(combined)

        if pyray.vector2_length(combined) < 1e-6:
            combined = hit_candidates[0][0]

        # Reflect velocity
        velocity = reflect(velocity, combined)

        # Small nudge to prevent sticking
        circle.center = pyray.vector2_add(circle.center, pyray.vector2_scale(combined, eps))

        # Remaining fraction of movement
        remaining *= (1.0 - earliest_t)

    return velocity  # return updated velocity
