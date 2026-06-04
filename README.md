# thegame
uma tentativa de aprender

# Calculo para saber o X e Y de uma imagem usando os dados do .tmj
tileId = gid - firstgid;
coluna = tileId % columns;
linha = tileId / columns;

sourceX = margin + coluna * (tileWidth + spacing);
sourceY = margin + linha * (tileHeight + spacing);

# Explicação dos tipos de tile de terreno para o projeto
Grass        = grama comum
Sand         = areia comum
Dirt         = terra comum / chão marrom
Water        = água profunda, não anda, não cava
ShallowWater = água rasa, talvez andável no futuro
Stone        = piso de pedra / chão duro
Path         = caminho colocado no mapa ou pelo player
WoodFloor    = piso de madeira
Bridge       = ponte sobre água
FarmSoil     = solo próprio de plantação, se você quiser separar de Grass/Sand
None         = vazio/desconhecido

# Estados do terreno
Normal  = tile normal
Dug     = cavado/arado
Wet     = molhado
Planted = com plantação
Blocked = bloqueado por algum sistema