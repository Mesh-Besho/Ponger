#/controls.py
import pyray as p
def update_controls(camera:p.Camera2D):
    #/controls.py[(function update_controls {camera:p.Camera2D})]
    global good_mouse_position
    m_pos = p.get_mouse_position()


    matrix = p.get_camera_matrix_2d(camera)
    iv_matrix = p.matrix_invert(matrix)
    good_mouse_position = p.vector2_transform(m_pos, iv_matrix)
