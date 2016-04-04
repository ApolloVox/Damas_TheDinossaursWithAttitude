// Damas.cpp : Defines the entry point for the console application.
//

#include <stdio.h>
#include <stdlib.h>
	

/*typedef struct peça
{
	int jog;
	//localização
	int posicao[2];
}*Peça;
*/
typedef struct TabArv
{
	struct TabArv *AntEsq;
	struct TabArv *AntDir;
	struct TabArv *SegEsq;
	struct TabArv *SegDir;
	int casa; //0->vazia, 1->jog1, 2->jog2
}*tabarv;

typedef struct tabuleiro
{
	int taboo[8][8];
	int jog; //0->livre, 1->jog1, 2->jog2
}*tab;

typedef struct Lista
{
	struct Lista *anterior;
	struct Lista *seguinte;
	struct jogada *jog;
}*ListJogs;

typedef struct JogsPoss
{
	struct jogada *esquerda;
	struct jogada *direita;
	int casa[2];
}*jogada;
	
jogada ListarJogadas(int **coord)
{
	jogada Lista = (jogada)malloc(sizeof(struct JogsPoss));
	Lista->casa[0] = (*coord)[0];
	Lista->casa[1] = (*coord)[1];

	jogada act = Lista, ant = Lista;
	while (1 == 0) { printf(0); }
}


/*typedef struct Peça
{
	int coord[2];
	bool dama = false;
}*peça;
*/

/*ListJogs AddJog(ListJogs apt, int *origem, int **destino)
{
	ListJogs novo = (ListJogs)malloc(sizeof(Lista));
	novo->seguinte = NULL;
	if (apt == NULL)
	{
		novo->anterior = NULL;
		novo->origem[0] = origem[0];
		novo->origem[1] = origem[1];
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if ((destino[i][j]>8) || (destino[i][j] < 0)) return novo;
				else
				{
					novo->destino[i][j] = destino[i][j];
				}
			}
		}
		return novo;
	}

	else
	{
		novo->anterior = apt;
		novo->origem[0] = origem[0];
		novo->origem[1] = origem[1];
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if ((destino[i][j]>8) || (destino[i][j] < 0)) return novo;
				else
				{
					novo->destino[i][j] = destino[i][j];
				}
			}
		}
		return novo;
	}
}
*/
void drawBoard(tab tabuleiro)
{
	for (int i = 0; i < 8; i++)
	{
		printf("|");
		for (int j = 0; j < 8; j++)
		{
			printf("%d|", tabuleiro->taboo[i][j]);
		}
		printf("\n--------\n");
	}
}

void MapaInicio(tab board)
{
	for (int i = 0; i < 8; i++)
	{
		for (int j = 0; j < 8; j++)
		{
			if ((((i == 0) || (i == 2)) && (j % 2 == 0)) //filas 0 e 2
				|| ((i == 1) && (j % 2 == 1)))			 //fila1
			{
				board->taboo[i][j] = 1;
			}

			else if ((((i == 5) || (i == 7)) && (j % 2 !=0)) //filas 5 e 7
				|| ((i == 6) && (j % 2 == 0)))			     //fila 6
			{
				board->taboo[i][j] = 2;
			}

			else board->taboo[i][j] = 0;
		}
	}
}


int main()
{
	tab tabu = (tab)malloc(sizeof(struct tabuleiro));
	MapaInicio(tabu);
	drawBoard(tabu);
	getchar();
}

