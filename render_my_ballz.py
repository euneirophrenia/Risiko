import bpy

# get the path where the blend file is located
path = bpy.path.abspath("G:\\Projects\\PROGETTO_UNI\\3D_Objects\\")

# deselect all objects
bpy.ops.object.select_all(action='DESELECT')    

# loop through all the objects in the scene
scene = bpy.context.scene
for ob in scene.objects:
    # make the current object active and select it
    scene.objects.active = ob
    ob.select = True
    old_extrude=ob.data.extrude
    ob.data.extrude=0.05
    # make sure that we only export meshes
    if ob.type == 'CURVE':
        # export the currently selected object to its own file based on its name
        bpy.ops.export_scene.obj(filepath=str(path + ob.name + '.obj'), use_selection=True)
    # deselect the object and move on to another if any more are left
    ob.select = False
    ob.data.extrude=old_extrude