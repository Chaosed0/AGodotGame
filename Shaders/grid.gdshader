shader_type canvas_item;

varying vec2 world_position;
varying vec4 modulate;

void vertex()
{
    world_position = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
    modulate = COLOR;
}

void fragment()
{
    const float spacing = 16.0;
    vec2 feh = world_position - (spacing * 0.5);
    vec2 grid = ceil(feh / spacing);
    float black_white = mod(grid.x + grid.y, 2);
    COLOR = vec4(black_white, black_white, black_white, 1) * modulate;
}
