## Funciones LUA

### MOD(name)
Inicia el mod con el nombre indicado, debe ser ejecutado en la primera línea

### **add_language(path)
Agrega un idioma dada la ruta a su archivo `.json`

### add_command(name, luaFunc, help)
Agrega un comando de consola

### add_credits(quantity)
Agrega una cantidad de créditos

### add_item(_id, quantity)
Agrega un item a tu inventario

### create_scene(name)
Crea una nueva escena

### create_object(name)
Crea y devuelve un nuevo objeto con el nombre indicado

### config(section, key, default, description)
Agrega una configuración

### create_inventory_item(item)
Crea un nuevo item para inventarios
Parametros: {name, itemType, spritePath, buyPrice, sellPrice, canBeStacked, canBeDestroyed, matterPercent}
```lua
item = {
	name = , -- nombre 
	
	-- puede tomar los siguientes valores 'RESOURCE', 'TOOL', 'SEED', 'CRAFTING', 'FISH', 'INSECT', 'PLACEABLE'
	type = , -- tipo
	sprite = , -- ruta del sprite
	buy_price = , -- precio de compra
	sell_price = , -- precio de venta
	stackable = , -- si puede acumularse en un único slot de inventario
	destroyable = , -- si puede ser destruido
	matter_percent = , cantidad de combustible que le da a la nave
}
```
### create_plant
Crea un nuevo item para inventarios
Parametros: (name, daysForDeath, daysForStage, growSeason, resources, seedSprite, s1Sprite, s2Sprite, s3Sprite, s4Sprite, s5Sprite)

### create_seed(inventoryId, plantsId)
Crea una semilla asignando un objeto de inventario a una lista de plantas

### execute_command(command)
Ejecuta un comando

### find_object(name)
Devuelve un objeto de la escena con nuevas funciones

### get_input(action)
Obtiene si un input se está pulsando

### get_object(path)
Obtiene un objeto especificando la ruta de objetos

### load_scene(scene)
Carga una escena indicada

### potrait_override(origin, path)
Cambia la textura de un portrait por la textura indicada con el `path`

### print(text)
Muesta por terminal el mensaje indicado

### print_scene()
Muesta por terminal la escena actual

### texture_override(path)
Cambia la textura indicada con el nombre del archivo `path`

### texture_override(origin, path)
Cambia la textura origin por la textura indicada con el `path`

### texture_override_in(path)
Cambia todas las texturas de una carpeta indicadas con el nombre de cada archivo

### toggle_ui()
Desactiva la interfaz de usuario **OCASIONA ERRORES**

### translate_inventory_item(id, names, descriptions)
Agrega traducciones a un inventory items

## Lua GameObject

### add_component(component)
Agrega el componente indicado al objeto

### add_position(x, y, z)
Agrega posición al objeto

### get_language()
Devuelve el idioma actual

### get_position()
Devuelve la posición del objeto

### set_position(x, y, z)
Cambia la posición del objeto

### toggle_active()
Cambia su estado activo