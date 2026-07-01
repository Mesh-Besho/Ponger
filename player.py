import info
import entities.mouse_magnet_powerup as MMpowerup
class player:
    def __init__(self, balls_left=2):
        self.items = []
        self.balls_left = balls_left
        self.magnet_power = info.MAX_MAGNET_POWER
        self.default_charge_amount = (1.0/3.0)
        self.charge_amount = self.default_charge_amount
        self.allowed_to_increase = True

    def charge_magnet(self, dt):
        self.magnet_power += self.charge_amount * dt
        if self.magnet_power > info.MAX_MAGNET_POWER:
            self.magnet_power = info.MAX_MAGNET_POWER
        
    def de_charge_magnet(self, dt):
        self.magnet_power -= dt
        if self.magnet_power < 0:
            self.magnet_power = 0.0

    def collect_item(self, item):
        self.items.append(item)


    def lose_item(self, item):
        if item in self.items:
            self.items.remove(item)

    def find_item(self, item:str):
        for X in self.items:
            if X.obj_id == item:
                return X
        return None
    
    def lose_spare_ball(self):
        x = True
        if self.balls_left > 0:
            self.balls_left -= 1
            x = False
        return x

    def update(self, dt):
        for x in self.items:
            x.update(dt)
        if self.does_player_have_item_type(MMpowerup.mouse_magnet_powerup):
            if self.allowed_to_increase:
                self.charge_amount *= self.default_charge_amount / 3
                self.allowed_to_increase = False
            
        
        
    
    def does_player_have_item_id(self, item_id:str):
            if self.find_item(item_id) is not None:
                return True
            else:
                return False
            
    def does_player_have_item_type(self, item_type:str):
        for x in self.items:
            if x.type == item_type:
                return True
        return False