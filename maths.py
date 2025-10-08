import pyray as p
from typing import Union

def get_intersection(a:p.Vector2, b:p.Vector2, c:p.Vector2, d:p.Vector2) -> Union[p.Vector2, None]:
    # Calculate direction vectors
    r = p.Vector2(b.x - a.x, b.y - a.y)
    s = p.Vector2(d.x - c.x, d.y - c.y)

    # Cross product of r and s
    rxs = r.x * s.y - r.y * s.x
    if rxs == 0:
        # Lines are parallel or colinear
        return None

    # Vector from a to c
    ac = p.Vector2(c.x - a.x, c.y - a.y)

    # Cross products to find t and u
    t = (ac.x * s.y - ac.y * s.x) / rxs
    u = (ac.x * r.y - ac.y * r.x) / rxs

    # Check if intersection lies on both segments
    if 0 <= t <= 1 and 0 <= u <= 1:
        intersection = p.Vector2(a.x + t * r.x, a.y + t * r.y)
        return intersection

    return None

def is_in_circle(c:p.Vector2, r:float, m:p.Vector2):
    how_far = p.vector2_distance(c, m)

    if how_far <= r:
        return True
    else:
        return False
