// Damas.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
	

typedef struct jogador
{
	int retroceder;
	//int peçasComidas;
	char peca;
}*jog;

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

void jogadas(tab board, char jog, int linha, int coluna, tabarv tb,int come) //se comeu uma peça 1, se não comeu 0
{
	if ((board->taboo[linha][coluna]=='0') && (come == 0)) printf("Casa nao e jogavel");
	else if ((jog != board->taboo[linha][coluna]) && (come == 0)) printf("Peca nao e do jogador.");
	else
	{
		if(come == 0)
			printf("Jogadas possiveis da peça %c em [%d,%d]:\n", jog, linha, coluna);
		//Jogador Branco
		if (jog == 'b')
		{
			//Esquerda
			if ((linha - 1 < 0) || (coluna - 1<0) || (board->taboo[linha - 1][coluna - 1] == 'b')) printf("");
			else
			{
				if ((come == 0) && (board->taboo[linha - 1][coluna - 1] == '0')) printf("-Para [%d,%d]\n", linha - 1, coluna - 1);
				else if ((board->taboo[linha - 1][coluna - 1] == 'p') && (linha - 2 >= 0) && (coluna - 2 >= 0) && (board->taboo[linha - 2][coluna - 2] == '0'))
				{
					if (come == 0)
					{
						printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha - 1, coluna - 1, linha - 2, coluna - 2);
						jogadas(board, jog, linha - 2, coluna - 2,tb, 1);
					}
					else if (come != 0)
					{
						for (int i = 0; i < come; i++) printf("\t");
						printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha - 1, coluna - 1, linha - 2, coluna - 2);
						jogadas(board, jog, linha - 2, coluna - 2,tb, come++);
					}
				}

			}
			//Direita
			if ((linha - 1 > 7) || (coluna + 1 < 0) || (board->taboo[linha - 1][coluna + 1] == 'b')) printf("");
			else if ((come == 0) && (board->taboo[linha - 1][coluna + 1] == '0')) printf("-Para [%d,%d]\n", linha - 1, coluna + 1);
			else if ((board->taboo[linha - 1][coluna + 1] == 'p') && (linha - 2 < 8) && (coluna + 2 >= 0) && (board->taboo[linha - 2][coluna + 2] == '0'))
			{
				if (come == 0)
				{
					printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha - 1, coluna + 1, linha - 2, coluna + 2);
					jogadas(board, jog, linha - 2, coluna + 2,tb, 1);
				}
				else if (come != 0)
				{
					for (int i = 0; i < come; i++) printf("\t");
					printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna - 1, linha - 2, coluna + 2);
					jogadas(board, jog, linha - 2, coluna + 2,tb, come++);
				}
			}

		}
		//Jogador preto
		if (jog == 'p')
		{
			//Esquerda
			if ((linha + 1 < 0) || (coluna - 1 > 7) || (board->taboo[linha + 1][coluna - 1] == 'p')) printf("");
			else
			{
				if ((come == 0) && (board->taboo[linha + 1][coluna - 1] == '0')) printf("-Para [%d,%d]\n", linha + 1, coluna - 1);
				else if ((board->taboo[linha + 1][coluna - 1] == 'b') && (linha + 2 >= 0) && (coluna - 2 < 8) && (board->taboo[linha + 2][coluna - 2] == '0'))
				{
					if (come == 0)
					{
						printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna - 1, linha + 2, coluna - 2);
						jogadas(board, jog, linha + 2, coluna - 2,tb, 1);
					}
					else if (come != 0)
					{
						for (int i = 0; i < come; i++) printf("\t");
						printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna - 1, linha + 2, coluna - 2);
						jogadas(board, jog, linha + 2, coluna - 2,tb, come++);
					}
				}				
			}
			//Direita
			if ((linha + 1 > 7) || (coluna + 1 > 7) || (board->taboo[linha + 1][coluna + 1] == 'p')) printf("Nada\n");
			else
			{
				if ((come == 0) && (board->taboo[linha + 1][coluna + 1] == '0')) printf("-Para [%d,%d]\n", linha + 1, coluna + 1);
				else if ((board->taboo[linha + 1][coluna + 1] == 'b') && (linha + 2 >= 0) && (coluna + 2 < 8) && (board->taboo[linha + 2][coluna + 2] == '0'))
				{
					if (come == 0)
					{
						printf("-Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna + 1, linha + 2, coluna + 2);
						jogadas(board, jog, linha + 2, coluna + 2,tb, 1);
					}
					else if (come != 0)
					{
						for (int i = 0; i < come; i++) printf("\t");
						printf("+Comer a peca em [%d,%d] e ir para [%d,%d]\n", linha + 1, coluna + 1, linha + 2, coluna + 2);
						jogadas(board, jog, linha + 2, coluna + 2,tb, come++);
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
	{ '0', '0', '0', 'p', 'p', '0', 'p', '0' },
	{ '0', 'p', '0', '0', '0', '0', '0', '0' },
	{ '0', '0', '0', '0', '0', 'p', '0', '0' },
	{ 'b', '0', 'b', '0', 'b', '0', 'b', '0' },
	{ '0', 'b', '0', 'b', '0', '0', '0', '0' },
	{ 'b', '0', 'b', '0', 'b', '0', 'b', '0' }
	};

	for (int i = 0; i < 8; i++)
	{
		for (int j = 0; j < 8; j++)
		{
			board->taboo[i][j] = b[i][j];
		}
	}	
}


int main()
{
	jog jogs[2];
	
	char aux[2] = { 'p','b' };
	int i;	
	for (i = 0; i < 2; i++)
	{
		jogs[i] = (jog)malloc(sizeof(struct jogador));
		jogs[i]->retroceder = 3;
		jogs[i]->peca = aux[i];
	}

	tabarv tb = (tabarv)malloc(sizeof(struct TabArv));
	tab tabu = (tab)malloc(sizeof(struct tabuleiro));
	ultimoTab lt = (ultimoTab)malloc(sizeof(struct lastBoard));	
	MapaInicio(tabu);

	/*drawBoard(tabu);*/
	/*lt = saveLast(lt, tabu);
	drawBoard2(lt);
	tabu->taboo[2][2] = 'b';
	drawBoard(tabu);
	tabu = retroceder(lt, tabu);
	int coord[2];*/
	printf("Indique as coordenadas da peça que quer jogar\n");				
	
	drawBoard(tabu);	
	jogadas(tabu, 'b', 5,6,tb,0);



	getchar();
}

