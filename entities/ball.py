#i@:
import pyray as p
from entities.sprite import sprite
from level import level
from wall import wall
import maths

MOUSE_MAGNET_RANGE = 200.1
MOUSE_MAGNET_POWER = 5.1

#c@:
class ball(sprite):
    #c':
    def __init__(self, level:level):
        t = p.load_texture("ball.png")
        super().__init__(t)
        self.W = 10
        self.H = 10
        self.direction = p.Vector2(111, 110)
        self.level = level
        self.radius = 10
    
    def update(self, dt):
        dt = dt / 10.0

        super().update(dt)
        #that's why we've got `dt`

        if p.is_mouse_button_down(p.MouseButton.MOUSE_BUTTON_LEFT):
            self.apply_magnet(dt, MOUSE_MAGNET_RANGE, MOUSE_MAGNET_POWER, p.get_mouse_position(), 1)

        elif p.is_mouse_button_down(p.MouseButton.MOUSE_BUTTON_RIGHT):
            self.apply_magnet(dt, MOUSE_MAGNET_RANGE, MOUSE_MAGNET_POWER, p.get_mouse_position(), -1)
        
        self.move_and_bounce(dt)

    def circle_segment_collision(self, circle_pos, radius, seg_a, seg_b):
        # Vector from seg_a to seg_b
        seg_v = p.vector2_subtract(seg_b, seg_a)
        # Vector from seg_a to circle center
        pt_v = p.vector2_subtract(circle_pos, seg_a)

        seg_len = p.vector2_length(seg_v)
        seg_dir = p.vector2_normalize(seg_v)

            # Project point vector onto segment
        proj = p.vector2_dot_product(pt_v, seg_dir)
        proj = max(0, min(seg_len, proj))  # Clamp projection to segment length

            # Closest point on the segment to the circle
        closest = p.vector2_add(seg_a, p.vector2_scale(seg_dir, proj))

            # Vector from closest point to circle center
        dist_v = p.vector2_subtract(circle_pos, closest)
        dist = p.vector2_length(dist_v)

        return dist <= radius



    def move_and_bounce(self, dt):
        import math
        radius = self.radius  # Make sure your object has a `.radius` attribute
        a = self.get_location()
        b = p.vector2_add(a, p.vector2_scale(self.direction, dt))

        walls = list(self.level.walls)
        for door in self.level.doors:
            for surface in door.surfaces:
                moved_wall = door.move_wall(surface)
                walls.append(moved_wall)

        for wall in walls:
            for i in range(len(wall.vertices)):
                start = wall.vertices[i]
                end = wall.vertices[(i + 1) % len(wall.vertices)]

                if self.circle_segment_collision(b, radius, start, end):
                    wall.colour = p.BLUE

                    # Reflect the direction vector
                    edge = p.vector2_subtract(end, start)
                    edge_normalized = p.vector2_normalize(edge)
                    normal = p.vector2_rotate(edge_normalized, math.pi / 2.0)
                    self.direction = p.vector2_reflect(self.direction, normal)

                    return  # stop after bounce

        self.set_location(b)


    def apply_magnet(self, dt:float, range:float, power:float, position:p.Vector2, polarity:float):
        magnetism = self.calculate_magnetism(range, power, position, polarity)
        magnetism_per_frame = p.vector2_scale(magnetism, dt)
        self.direction = p.vector2_add(magnetism_per_frame, self.direction)

    def calculate_magnetism(self, range:float, power:float, position:p.Vector2, polarity:float):
        #self.direction
        location = self.get_location()
        v = p.Vector2(position.x - location.x, position.y - location.y)
        distance = p.vector2_length(v)

        actual_power = power * (range - distance)
        actual_power = self.minimise(actual_power, 0)
        direction = p.vector2_normalize(v)
        magnetism = p.vector2_scale(direction, polarity)
        magnetism = p.vector2_scale(magnetism, actual_power)
        return magnetism


    def minimise(self, var, minimum):
        if var < minimum:
            var = minimum
        return var        
    def maximise(self, var, maximum):
        if var < maximum:
            var = maximum
        return var        
        

    def draw(self):
        #super().draw()
        
        p.draw_circle_lines(int(self.X), int(self.Y), self.radius, p.BLACK)

        a = self.get_location()
        b = p.vector2_add(a, p.vector2_scale(self.direction, 10))
        p.draw_line(int(self.X), int(self.Y), int(b.x), int(b.y), p.GREEN)
                    

    
#thing_as_class = class(thing)