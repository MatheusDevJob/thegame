# thegame
uma tentativa de aprender

# Calculo para saber o X e Y de uma imagem usando os dados do .tmj
tileId = gid - firstgid;
coluna = tileId % columns;
linha = tileId / columns;

sourceX = margin + coluna * (tileWidth + spacing);
sourceY = margin + linha * (tileHeight + spacing);