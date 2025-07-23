import pyray as p

print("HERE IS WHATEVER i WANT, 95225ABCDEFGHIJKLMNOPQRSTUVWXyZ 7rk se77rvt b7tb wuretu GY uyK KI VUKU uyk D tju FV\fyk kfgbfsb sjsgsdvuygkbyiwycsdygPNAME")


p.init_window(800, 600, b"Ponger")
p.init_audio_device()
#p.toggle_fullscreen()
p.set_target_fps(30)

#game.mesh_besho()


while not p.window_should_close():
    dt = p.get_frame_time()

    p.begin_drawing()
    p.clear_background(p.WHITE)
    
    #game.update(dt)

    #for list in all_files.entities.get_all():
    #    list.update(dt)
    #    list.letsdraw()    
    
    p.end_drawing()
    

#5.h