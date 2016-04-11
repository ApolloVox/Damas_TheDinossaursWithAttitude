// Damas.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
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
	char taboo[8][8];	
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
	
typedef struct lastBoard
{
	char state[8][8];
	struct lastBoard *seguinte;
}*ultimoTab;



ultimoTab saveLast(ultimoTab last ,tab board)
{
	for (int i = 0; i < 8; i++)
	{
		for (int j = 0; j < 8; j++)
		{
			last->state[i][j] = board->taboo[i][j];
		}
	}
	return last;
}

tab retroceder(ultimoTab last, tab board)
{
	for (int i = 0; i < 8; i++)
	{
		for (int j = 0; j < 8; j++)
		{
			board->taboo[i][j] = last->state[i][j];
		}
	}
	return board;
}
 
void ListarJogadas(int **coord)
{
	jogada Lista = (jogada)malloc(sizeof(struct JogsPoss));
	Lista->casa[0] = (*coord)[0];
	Lista->casa[1] = (*coord)[1];

	jogada act = Lista, ant = Lista;
	while (1 == 0) { printf(0); }
}

void jogadas(tab board, char jog, int linha, int coluna, int come) //se comeu uma peça 1, se não comeu 0
{
	if (board->taboo[linha][coluna]==0) printf("Casa nao e jogavel");
	else if (jog != board->taboo[linha][coluna]) printf("Peca nao e do jogador.");
	else
	{
		printf("Jogadas possiveis da peça %c em [%d,%d]:\n", jog, linha, coluna);
		//Jogador Branco
		if (jog == 'b')
		{
			//Esquerda
			if ((linha - 1 < 0) || (coluna - 1<0) || (board->taboo[linha - 1][coluna - 1] == 'b')) printf("");
			else
			{
				if ((come == 0) && (board->taboo[linha - 1][coluna - 1] == '0')) printf("-Para [%d,%d]\n", linha - 1, coluna - 1);
				else if ((board->taboo[linha - 1][coluna - 1] == 'p') && (linha - 2 >= 0) && (coluna - 2 > 0) && (board->taboo[linha - 2][coluna - 2] == '0'))
				{
					if (come = 0)
					{
						printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha - 1, coluna - 1, linha - 2, coluna - 2);
						jogadas(board, jog, linha - 2, coluna - 2, 1);
					}
					else if (come != 0)
					{
						for (int i = 0; i < come; i++) printf("\t");
						printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha - 1, coluna - 1, linha - 2, coluna - 2);
						jogadas(board, jog, linha - 2, coluna - 2, come++);
					}
				}
				for (int i = 0; i < come; i++) printf("\t");
				printf("OU\n");
			}
			//Direita
			if ((linha + 1 > 7) || (coluna - 1 < 0) || (board->taboo[linha + 1][coluna - 1] == 'b')) printf("");
			else if ((come == 0) && (board->taboo[linha + 1][coluna - 1] == '0')) printf("-Para [%d,%d]\n", linha + 1, coluna - 1);
			else if ((board->taboo[linha + 1][coluna - 1] == 'p') && (linha + 2 < 8) && (coluna + 2 > 0) && (board->taboo[linha + 2][coluna - 2] == '0'))
			{
				if (come = 0)
				{
					printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna - 1, linha + 2, coluna - 2);
					jogadas(board, jog, linha + 2, coluna - 2, 1);
				}
				else if (come != 0)
				{
					for (int i = 0; i < come; i++) printf("\t");
					printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna - 1, linha + 2, coluna - 2);
					jogadas(board, jog, linha + 2, coluna - 2, 1);
				}
			}

		}
		//Jogador preto
		if (jog == 'p')
		{
			//Esquerda
			if ((linha - 1 < 0) || (coluna + 1 > 7) || (board->taboo[linha - 1][coluna + 1] == 'p')) printf("");
			else
			{
				if ((come == 0) && (board->taboo[linha - 1][coluna + 1] == '0')) printf("-Para [%d,%d]\n", linha - 1, coluna + 1);
				else if ((board->taboo[linha - 1][coluna + 1] == 'b') && (linha - 2 >= 0) && (coluna + 2 < 8) && (board->taboo[linha - 2][coluna + 2] == '0'))
				{
					if (come = 0)
					{
						printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha - 1, coluna + 1, linha - 2, coluna + 2);
						jogadas(board, jog, linha - 2, coluna + 2, 1);
					}
					else if (come != 0)
					{
						for (int i = 0; i < come; i++) printf("\t");
						printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha - 1, coluna + 1, linha - 2, coluna + 2);
						jogadas(board, jog, linha - 2, coluna + 2, come++);
					}
				}
				for (int i = 0; i < come; i++) printf("\t");
				printf("OU\n");
			}
			//Direita
			if ((linha + 1 > 7) || (coluna + 1 > 7) || (board->taboo[linha + 1][coluna + 1] == 'p')) printf("Nada\n");
			else
			{
				if ((come == 0) && (board->taboo[linha + 1][coluna + 1] == '0')) printf("-Para [%d,%d]\n", linha + 1, coluna + 1);
				else if ((board->taboo[linha + 1][coluna + 1] == 'b') && (linha + 2 >= 0) && (coluna + 2 < 8) && (board->taboo[linha + 2][coluna + 2] == '0'))
				{
					if (come = 0)
					{
						printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna + 1, linha + 2, coluna + 2);
						jogadas(board, jog, linha + 2, coluna + 2, 1);
					}
					else if (come != 0)
					{
						for (int i = 0; i < come; i++) printf("\t");
						printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna + 1, linha + 2, coluna + 2);
						jogadas(board, jog, linha + 2, coluna + 2, come++);
					}
				}
			}

		}
	}
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
			printf("%c|", tabuleiro->taboo[i][j]);
		}
		printf(" %d\n",i);
	}
	printf("-----------------\n 0/1/2/3/4/5/6/7\n\n");
}
void drawBoard2(ultimoTab tabuleiro)
{	
	for (int i = 0; i < 8; i++)
	{
		printf("|");
		for (int j = 0; j < 8; j++)
		{
			printf("%c|", tabuleiro->state[i][j]);
		}
		printf(" %d\n", i);
	}
	printf("-----------------\n 0/1/2/3/4/5/6/7\n\n");
}

void MapaInicio(tab board)
{
	char b[8][8] = {
	{ 'p', '0', 'p', '0', 'p', '0', 'p', '0' },
	{ '0', 'p', '0', 'p', '0', 'p', '0', 'p' },
	{ 'p', '0', 'p', '0', 'p', '0', 'p', '0' },
	{ '0', '0', '0', '0', '0', '0', '0', '0' },
	{ '0', '0', '0', '0', '0', '0', '0', '0' },
	{ 'b', '0', 'b', '0', 'b', '0', 'b', '0' },
	{ '0', 'b', '0', 'b', '0', 'b', '0', 'b' },
	{ 'b', '0', 'b', '0', 'b', '0', 'b', '0' }
	};

	for (int i = 0; i < 8; i++)
	{
		for (int j = 0; j < 8; j++)
		{
			board->taboo[i][j] = b[i][j];
		}
	}

	/*
	for (int i = 0; i < 8; i++)
	{
		for (int j = 0; j < 8; j++)
		{
			if ((((i == 0) || (i == 2)) && (j % 2 == 0)) //filas 0 e 2
				|| ((i == 1) && (j % 2 == 1)))			 //fila1
			{
				board->taboo[i][j] = 'p';
			}

			else if ((((i == 5) || (i == 7)) && (j % 2 != 0)) //filas 5 e 7
				|| ((i == 6) && (j % 2 == 0)))			     //fila 6
			{
				board->taboo[i][j] = 'b';
			}

			else board->taboo[i][j] = '0';
		}
	}
*/
}


int main()
{
	tab tabu = (tab)malloc(sizeof(struct tabuleiro));
	ultimoTab lt = (ultimoTab)malloc(sizeof(struct lastBoard));
	MapaInicio(tabu);
	drawBoard(tabu);
	lt = saveLast(lt, tabu);
	drawBoard2(lt);
	tabu->taboo[2][2] = 'b';
	drawBoard(tabu);
	tabu = retroceder(lt, tabu);
	int coord[2];
	printf("Indique as coordenadas da peça que quer jogar\n");				
	
	drawBoard(tabu);	
	jogadas(tabu, 'p', 2,2,0);



	getchar();
}

