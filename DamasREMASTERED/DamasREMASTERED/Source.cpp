
#include<stdlib.h>
#include<stdio.h>
#include<string.h>

typedef struct TabArv
{
	struct TabArv *AntEsq;
	struct TabArv *AntDir;
	struct TabArv *SegEsq;
	struct TabArv *SegDir;
	int casa[2]; //0->vazia, 1->jog1, 2->jog2
	int comestivel;
	int move;
	int dama;
}*tabarv;

typedef struct tabuleiro
{
	char taboo[8][8];
	struct tabuleiro *anterior;
	struct tabuleiro *seguinte;
}*tab;

tabarv jogadas(tab board, char peca, int linha, int coluna, int come, int move) //se comeu uma peça 1, se não comeu 0
{
	if (move == 5)
	{
		return NULL;
	}
	else
	{
		tabarv tb = (tabarv)malloc(sizeof(struct TabArv));
		tb->casa[0] = linha;
		tb->casa[1] = coluna;

		//Transforma em Dama
		if (peca == 'b'&& linha == 0)
		{
			tb->dama = 1;
			peca = 'B';
		}
		else if (peca == 'p'&&coluna == 7)
		{
			tb->dama = 1;
			peca = 'P';
		}
		else tb->dama = 0;

		tb->move = move;
		if (move == 0)
			tb->comestivel = -1;
		else tb->comestivel = come;

		//Jogador Branco
		if ((peca == 'b') || (peca == 'B'))
		{
			if (peca == 'b') //Não dama
			{
				tb->SegDir = NULL;
				tb->SegEsq = NULL;
			}
			else //Dama (Casas para trás)
			{
				//Esquerda
				if ((linha + 1 > 7) || (coluna - 1 < 0) ||  //Verifica se casa existe 
					(board->taboo[linha + 1][coluna - 1] == 'b' || (board->taboo[linha + 1][coluna - 1] == 'B'))) //não está ocupda
				{
					tb->SegEsq = NULL;
				}
				else
				{
					if (board->taboo[linha + 1][coluna - 1] == '0')
					{
						tb->SegEsq = jogadas(board, peca, linha + 1, coluna - 1, come, move + 1);
						if (tb->SegEsq != NULL)
							tb->SegEsq->AntDir = tb;
					}

					else if (((board->taboo[linha + 1][coluna - 1] == 'p') || (board->taboo[linha + 1][coluna - 1] == 'P'))
						&& (linha + 2 < 8) && (coluna - 2 >= 0) && (board->taboo[linha + 2][coluna - 2] == '0'))
					{
						tb->SegEsq = jogadas(board, peca, linha + 2, coluna - 2, come + 1, move + 1);
						if (tb->SegEsq != NULL)
							tb->SegEsq->AntDir = tb;
					}
					else
					{
						tb->SegEsq = NULL;
					}
				}

				//Direita
				if ((linha + 1 > 7) || (coluna + 1 > 7) ||  //Verifica se casa existe 
					(board->taboo[linha + 1][coluna + 1] == 'b' || (board->taboo[linha + 1][coluna + 1] == 'B'))) //não está ocupda
				{
					tb->SegDir = NULL;
				}
				else
				{
					if (board->taboo[linha + 1][coluna + 1] == '0')
					{
						tb->SegDir = jogadas(board, peca, linha + 1, coluna + 1, come, move + 1);
						if (tb->SegDir != NULL)
							tb->SegDir->AntEsq = tb;
					}

					else if (((board->taboo[linha + 1][coluna + 1] == 'p') || (board->taboo[linha + 1][coluna + 1] == 'P'))
						&& (linha + 2 < 8) && (coluna + 2 < 8) && (board->taboo[linha + 2][coluna + 2] == '0'))
					{
						tb->SegDir = jogadas(board, peca, linha + 2, coluna + 2, come + 1, move + 1);
						if (tb->SegDir != NULL)
							tb->SegDir->AntEsq = tb;
					}
					else
					{
						tb->SegDir = NULL;
					}
				}

			}
			//Esquerda
			if ((linha - 1 < 0) || (coluna - 1 < 0) ||  //Verifica se casa existe 
				(board->taboo[linha - 1][coluna - 1] == 'b' || (board->taboo[linha - 1][coluna - 1] == 'B'))) //não está ocupda
			{
				tb->AntEsq = NULL;
			}
			else
			{
				if (board->taboo[linha - 1][coluna - 1] == '0')
				{
					tb->AntEsq = jogadas(board, peca, linha - 1, coluna - 1, come, move + 1);
					if (tb->AntEsq != NULL)
						tb->AntEsq->SegDir = tb;
				}

				else if (((board->taboo[linha - 1][coluna - 1] == 'p') || (board->taboo[linha - 1][coluna - 1] == 'P'))
					&& (linha - 2 >= 0) && (coluna - 2 >= 0) && (board->taboo[linha - 2][coluna - 2] == '0'))
				{
					tb->AntEsq = jogadas(board, peca, linha - 2, coluna - 2, come + 1, move + 1);
					if (tb->AntEsq != NULL)
						tb->AntEsq->SegDir = tb;
				}
				else
				{
					tb->AntEsq = NULL;
				}
			}

			//Direita
			if ((linha - 1 < 0) || (coluna + 1 > 7) ||  //Verifica se casa existe 
				(board->taboo[linha - 1][coluna + 1] == 'b' || (board->taboo[linha - 1][coluna + 1] == 'B')))
			{
				tb->AntDir = NULL;
			}

			else if (board->taboo[linha - 1][coluna + 1] == '0')
			{
				tb->AntDir = jogadas(board, peca, linha - 1, coluna + 1, come, move + 1);
				if (tb->AntDir != NULL)
					tb->AntDir->SegEsq = tb;
			}
			else if (((board->taboo[linha - 1][coluna + 1] == 'p') || (board->taboo[linha - 1][coluna + 1] == 'P'))
				&& (linha - 2 >= 0) && (coluna + 2 < 8) && (board->taboo[linha - 2][coluna + 2] == '0'))
			{
				tb->AntDir = jogadas(board, peca, linha - 2, coluna + 2, come + 1, move + 1);
				if (tb->AntDir != NULL)
					tb->AntDir->SegEsq = tb;
			}
			else tb->AntDir = NULL;
		}

		//Jogador preto
		if ((peca == 'p') || (peca == 'P'))
		{
			if (peca == 'p') //Não dama
			{
				tb->AntDir = NULL;
				tb->AntEsq = NULL;
			}
			else //Dama (Casas para trás)
			{
				//Esquerda
				if ((linha - 1 < 0) || (coluna - 1 < 0) ||  //Verifica se casa existe 
					(board->taboo[linha - 1][coluna - 1] == 'p' || (board->taboo[linha - 1][coluna - 1] == 'P'))) //não está ocupda
				{
					tb->AntEsq = NULL;
				}
				else
				{
					if (board->taboo[linha - 1][coluna - 1] == '0')
					{
						tb->AntEsq = jogadas(board, peca, linha - 1, coluna - 1, come, move + 1);
						if (tb->AntEsq != NULL)
							tb->AntEsq->SegDir = tb;						
					}

					else if (((board->taboo[linha - 1][coluna - 1] == 'b') || (board->taboo[linha - 1][coluna - 1] == 'B'))
						&& (linha - 2 >= 0) && (coluna - 2 >= 0) && (board->taboo[linha - 2][coluna - 2] == '0'))
					{
						tb->AntEsq = jogadas(board, peca, linha - 2, coluna - 2, come + 1, move + 1);
						if (tb->AntEsq != NULL)
							tb->AntEsq->SegDir = tb;						
					}
					else
					{
						tb->AntEsq = NULL;
					}
				}

				//Direita
				if ((linha - 1 < 0) || (coluna + 1 >7) ||  //Verifica se casa existe 
					(board->taboo[linha - 1][coluna + 1] == 'p' || (board->taboo[linha - 1][coluna + 1] == 'P'))) //não está ocupda
				{
					tb->AntDir = NULL;
				}
				else
				{
					if (board->taboo[linha - 1][coluna + 1] == '0')
					{
						tb->AntDir = jogadas(board, peca, linha - 1, coluna + 1, come, move + 1);
						if (tb->AntDir != NULL)
							tb->AntDir->SegEsq = tb;						
					}
					else if (((board->taboo[linha - 1][coluna + 1] == 'b') || (board->taboo[linha - 1][coluna + 1] == 'B'))
						&& (linha - 2 >= 0) && (coluna + 2 < 8) && (board->taboo[linha - 2][coluna + 2] == '0'))
					{
						tb->AntDir = jogadas(board, peca, linha - 2, coluna + 2, come + 1, move + 1);
						tb->AntDir->SegEsq = tb;
					}
					else
					{
						tb->AntDir = NULL;
					}
				}

			}
			//Esquerda
			if ((linha + 1 > 7) || (coluna - 1 < 0) ||  //Verifica se casa existe 
				(board->taboo[linha + 1][coluna - 1] == 'p' || (board->taboo[linha + 1][coluna - 1] == 'P'))) //não está ocupda
			{
				tb->SegEsq = NULL;
			}
			else
			{
				if (board->taboo[linha + 1][coluna - 1] == '0')
				{
					tb->SegEsq = jogadas(board, peca, linha + 1, coluna - 1, come, move + 1);
					if (tb->SegEsq != NULL)
						tb->SegEsq->AntDir = tb;					
				}

				else if (((board->taboo[linha + 1][coluna - 1] == 'b') || (board->taboo[linha + 1][coluna - 1] == 'B'))
					&& (linha + 2 <8) && (coluna - 2 >= 0) && (board->taboo[linha + 2][coluna - 2] == '0'))
				{
					tb->SegEsq = jogadas(board, peca, linha + 2, coluna - 2, come + 1, move + 1);
					if (tb->SegEsq != NULL)
						tb->SegEsq->AntDir = tb;					
				}
				else
				{
					tb->SegEsq = NULL;
				}
			}
			//Direita
			if ((linha + 1 >7) || (coluna + 1 > 7) ||  //Verifica se casa existe 
				(board->taboo[linha + 1][coluna + 1] == 'p' || (board->taboo[linha + 1][coluna + 1] == 'P')))
			{
				tb->SegDir = NULL;
			}
			else if (board->taboo[linha + 1][coluna + 1] == '0')
			{
				tb->SegDir = jogadas(board, peca, linha + 1, coluna + 1, come, move + 1);
				if (tb->SegDir != NULL)
					tb->SegDir->AntEsq = tb;				
			}
			else if (((board->taboo[linha + 1][coluna + 1] == 'b') || (board->taboo[linha + 1][coluna + 1] == 'B'))
				&& (linha + 2 <8) && (coluna + 2 <8) && (board->taboo[linha + 2][coluna + 2] == '0'))
			{
				tb->SegDir = jogadas(board, peca, linha + 2, coluna + 2, come + 1, move + 1);
				if (tb->SegDir != NULL)
					tb->SegDir->AntEsq = tb;			
			}
			else tb->SegDir = NULL;
		}
		return tb;
	}
}

//verificar se nos movimentos possiveis existe algum comer possivel
int maxComestivel(tabarv tb)
{
	int max = tb->comestivel;

	if ((tb->AntDir != NULL) && (tb->AntDir->comestivel > max)) max = tb->AntDir->comestivel;
	if ((tb->AntEsq != NULL) && (tb->AntEsq->comestivel > max)) max = tb->AntEsq->comestivel;
	if ((tb->SegDir != NULL) && (tb->SegDir->comestivel > max)) max = tb->SegDir->comestivel;
	if ((tb->SegEsq != NULL) && (tb->SegEsq->comestivel > max)) max = tb->SegEsq->comestivel;

	return max;
}


//print dos caminhos possiveis
void printCaminhos(tabarv tb)
{
	if (tb != NULL)
	{
		int comes = maxComestivel(tb);

		if (tb->AntEsq != NULL && tb->AntEsq->move>tb->move && tb->AntEsq->comestivel == comes)
			printCaminhos(tb->AntEsq);
		if (tb->AntDir != NULL && tb->AntDir->move>tb->move && tb->AntDir->comestivel == comes)
			printCaminhos(tb->AntDir);

		int i;
		for (i = -1; i < tb->move; i++) printf("\t");			//tabs para ficar agradavel à vista as jogadas possiveis
			printf("[%d,%d]\n", tb->casa[0], tb->casa[1]);

		if (tb->SegEsq != NULL && tb->SegEsq->move>tb->move && tb->SegEsq->comestivel == comes)
			printCaminhos(tb->SegEsq);
		if (tb->SegDir != NULL && tb->SegDir->move>tb->move && tb->SegDir->comestivel == comes)
			printCaminhos(tb->SegDir);
	}
}

//array que contem o caminho todo
int caminho[5];

void escolheCaminho(tabarv tb)
{
	tabarv aux = tb;
	int direc = -1, i = 0;
	printCaminhos(tb);
	printf("Indique o caminho da peca por ordem (Ex: 1(Enter)2(Enter)0(Enter)");
	printf("\n 1 - Cima Esquerdo\n 2 - Cima Direita\n 3 - Baixo Esquerdo\n 4 - Baixo Direito\n 0 - Fim jogada\n-1 - Jogar outra peca\n");
	do
	{
		int b = 0;

		do
		{
			scanf("%d", &direc);
			
			//AntEsq
			if (direc == 1)
			{
				if (tb->AntEsq != NULL)
				{
					b = 1;
					tb = tb->AntEsq;
				}
				else printf("Jogada invalida. Tente de novo.\n");
			}

			//AntDir
			else if (direc == 2)
			{
				if (tb->AntDir != NULL)
				{
					b = 1;
					tb = tb->AntDir;
				}
				else printf("Jogada invalida. Tente de novo.\n");
			}

			//SegEsq
			else if (direc == 3)
			{
				if (tb->SegEsq != NULL)
				{
					b = 1;
					tb = tb->SegEsq;
				}
				else printf("Jogada invalida. Tente de novo.\n");
			}

			//SegDir
			else if (direc == 4)
			{
				if (tb->SegDir != NULL)
				{
					b = 1;
					tb = tb->SegDir;
				}
				else printf("Jogada invalida. Tente de novo.\n");
			}

			else b = 1;

		} while (b == 0);
		
		//verificar direc
		if (direc == -1)
		{
			caminho[1] = -1;
			break;
		}
		if (direc > 4 || direc < 1)
		{
			caminho[i] = 0;
			break;
		}

		caminho[i] = direc;
		i++;
	} while (i < 5);
}

void moverPeca(tab board, tabarv tb, char j)
{	
	int i = 0;
	char jog = j;
	while ((caminho[i] < 5) && (caminho[i] > 0) && (i < 5))
	{
		board->taboo[tb->casa[0]][tb->casa[1]] = '0';
		switch (caminho[i])
		{
			//AntEsq
			case 1:
			{
				if (tb->AntEsq->comestivel > 0) board->taboo[tb->casa[0] - 1][tb->casa[1] - 1] = '0';
				
				if ((jog == 'b') && (tb->AntEsq->casa[0] == 0))
				{
					board->taboo[tb->AntEsq->casa[0]][tb->AntEsq->casa[1]] = 'B';
					jog = 'B';
				}
				else board->taboo[tb->AntEsq->casa[0]][tb->AntEsq->casa[1]] = jog;
				
				tb = tb->AntEsq;
				break;
			}
			case 2:
			{
				if (tb->AntDir->comestivel > 0) board->taboo[tb->casa[0] - 1][tb->casa[1] + 1] = '0';
				if ((jog == 'b') && (tb->AntDir->casa[0] == 0))
				{
					board->taboo[tb->AntDir->casa[0]][tb->AntDir->casa[1]] = 'B';
					jog = 'B';
				}
				else board->taboo[tb->AntDir->casa[0]][tb->AntDir->casa[1]] = jog;
				tb = tb->AntDir;
				break;
			}
			case 3:
			{
				if (tb->SegEsq->comestivel > 0) board->taboo[tb->casa[0] + 1][tb->casa[1] - 1] = '0';
				if ((jog == 'p') && (tb->SegEsq->casa[0] == 7))
				{
					board->taboo[tb->SegEsq->casa[0]][tb->SegEsq->casa[1]] = 'P';
					jog = 'P';
				}
				else board->taboo[tb->SegEsq->casa[0]][tb->SegEsq->casa[1]] = jog;
				tb = tb->SegEsq;
				break;
			}
			case 4:
			{
				if (tb->SegDir->comestivel > 0) board->taboo[tb->casa[0] + 1][tb->casa[1] + 1] = '0';
				if ((jog == 'p') && (tb->SegDir->casa[0] == 7))
				{
					board->taboo[tb->SegDir->casa[0]][tb->SegDir->casa[1]] = 'P';
					jog = 'P';
				}
				else board->taboo[tb->SegDir->casa[0]][tb->SegDir->casa[1]] = jog;
				tb = tb->SegDir;
				break;
			}
			default:
			{
				printf("Não funcemina assim tente de novo.");
				break;
			}
		}

		i++;
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
void drawBoard(tab tabuleiro,int ant)
{
	int i, j;
	do
	{
		for (i = 0; i < 8; i++)
		{
			printf("|");
			for (j = 0; j < 8; j++)
			{
				printf("%c|", tabuleiro->taboo[i][j]);
			}
			printf(" %d\n", i);
		}
		printf("-----------------\n 0/1/2/3/4/5/6/7\n\n");

		if (ant == 0)		
			break;		
		tabuleiro = tabuleiro->anterior;
	} while (tabuleiro != NULL);	
}
void MapaInicio(tab board)
{
	int i, j;
	char b[8][8] = {
		{ '0', 'p', '0', 'p', '0', '0', '0', 'p' },
		{ 'p', '0', 'p', '0', '0', '0', 'b', '0' },
		{ '0', 'p', 'P', 'p', '0', 'p', '0', 'p' },
		{ '0', '0', '0', '0', '0', '0', '0', '0' },
		{ '0', '0', '0', '0', '0', '0', '0', '0' },
		{ 'b', '0', 'b', '0', 'b', '0', 'b', '0' },
		{ '0', 'b', '0', 'b', '0', 'b', '0', 'b' },
		{ 'b', '0', 'b', '0', 'b', '0', 'b', '0' }
	};
	for (i = 0; i < 8; i++)	
		for (j = 0; j < 8; j++)		
			board->taboo[i][j] = b[i][j];	
}


int semPecas(tab tabuleiro)
{
	int i, j, b = 0, p = 0;

	for (i = 0; i < 8; i++)
	{		
		for (j = 0; j < 8; j++)
		{
			if (tabuleiro->taboo[i][j] == 'b' || tabuleiro->taboo[i][j] == 'B')
				b++;
			if (tabuleiro->taboo[i][j] == 'p' || tabuleiro->taboo[i][j] == 'P')
				p++;
		}
	}
	if (b>0 && p>0)	
		return -1;	
	if (b <= 0)
		return 1;
	if (p <= 0)
		return 0;	
}


tab saveLastBoard(tab board)
{
	int i, j;
	tab tabu = (tab)malloc(sizeof(struct tabuleiro));
	for (i = 0; i < 8; i++)
		for (j = 0; j < 8; j++)
			tabu->taboo[i][j] = board->taboo[i][j];
	tabu->anterior = board;	
	tabu->seguinte = NULL;
	board->seguinte = tabu;	
	return tabu;
}

tab retrocederJogada(tab board)
{
	board = board->anterior;	
	return board;
}

int existe(char *fname)
{
	FILE *file;
	if (file = fopen(fname, "rb"))
	{
		fclose(file);
		return 1;
	}
	return 0;
}
void saveGameFile(tab board, int idJog, int r1, int r2,int nJog)
{
	fseek(stdin, 0, SEEK_END);
	int i, j;
	char str[100];
	char *fileName;
	int op = 0, bk = 0;
	do
	{
		fseek(stdin, 0, SEEK_END);
		system("cls");
		puts("Insira um nome para o ficheiro : ");
		scanf("%[^\n]s", str);
		fileName = strcat(str, ".bin");
		system("cls");
		if (existe(fileName) == 1)
		{
			puts("Ficheiro ja existe...\n-1 Substituir\n-2 Inserir novo nome\n-3 Nao salvar");
			scanf("%d", &op);
			if (op == 1)
			{
				FILE *newfile;
				newfile = fopen(fileName, "wb");
				tab tbAux = board;
				fwrite(&nJog, sizeof(int), 1, newfile);
				do
				{					
					for (i = 0; i < 8; i++)
					{
						for (j = 0; j < 8; j++)
						{
							char cAux = tbAux->taboo[i][j];
							int iAux;
							if (cAux == 'p')
								iAux = 0;
							else if (cAux == '0')
								iAux = 1;
							else if (cAux == 'b')
								iAux = 2;
							else if (cAux == 'B')
								iAux = 3;
							else if (cAux == 'P')
								iAux = 4;
							else iAux = 200;
							fwrite(&iAux, sizeof(int), 1, newfile); //.bin
						}
					}
					tbAux = tbAux->anterior;
				} while (tbAux != NULL);
				//for (i = 0; i < 8; i++)
				//{
				//	for (j = 0; j < 8; j++)
				//	{
				//		char cAux = board->anterior->taboo[i][j];
				//		int iAux;
				//		if (cAux == 'p')
				//			iAux = 0;
				//		else if (cAux == '0')
				//			iAux = 1;
				//		else if (cAux == 'b')
				//			iAux = 2;
				//		else if (cAux == 'B')
				//			iAux = 3;
				//		else if (cAux == 'P')
				//			iAux = 4;
				//		else iAux = 200;
				//		fwrite(&iAux, sizeof(int), 1, newfile); //.bin
				//	}
				//}
				fwrite(&idJog, sizeof(int), 1, newfile);
				fwrite(&r1, sizeof(int), 1, newfile);
				fwrite(&r2, sizeof(int), 1, newfile);
				fclose(newfile);
				puts("Save efetuado com sucesso !!");				
				break;
			}
			else if (op == 2)
			{
				bk = 1;
			}
			else if (op == 3)
			{
				puts("Terminado !!");				
				break;
			}
		}
		else
		{
			FILE *newfile;
			newfile = fopen(fileName, "wb");
			tab tbAux = board;
			do
			{
				for (i = 0; i < 8; i++)
				{
					for (j = 0; j < 8; j++)
					{
						char cAux = tbAux->taboo[i][j];
						int iAux;
						if (cAux == 'p')
							iAux = 0;
						else if (cAux == '0')
							iAux = 1;
						else if (cAux == 'b')
							iAux = 2;
						else if (cAux == 'B')
							iAux = 3;
						else if (cAux == 'P')
							iAux = 4;
						else iAux = 200;
						fwrite(&iAux, sizeof(int), 1, newfile); //.bin
					}
				}
				tbAux = tbAux->anterior;
			} while (tbAux != NULL);
			//for (i = 0; i < 8; i++)
			//{
			//	for (j = 0; j < 8; j++)
			//	{
			//		char cAux = board->anterior->taboo[i][j];
			//		int iAux;
			//		if (cAux == 'p')
			//			iAux = 0;
			//		else if (cAux == '0')
			//			iAux = 1;
			//		else if (cAux == 'b')
			//			iAux = 2;
			//		else if (cAux == 'B')
			//			iAux = 3;
			//		else if (cAux == 'P')
			//			iAux = 4;
			//		else iAux = 200;
			//		fwrite(&iAux, sizeof(int), 1, newfile); //.bin
			//	}
			//}
			fwrite(&idJog, sizeof(int), 1, newfile);//binario
			fwrite(&r1, sizeof(int), 1, newfile);
			fwrite(&r2, sizeof(int), 1, newfile);
			fclose(newfile);
			puts("Save efetuado com sucesso !!");			
			break;
		}
		fileName = "";
	} while (bk == 1);
}

tab inserirFim(tab board, tab last)
{
	int i,j;
	tab aux = board;
	tab novo = (tab)malloc(sizeof(struct tabuleiro));
	if (board == NULL)
	{
		for (i = 0; i < 8; i++)	
			for (j = 0; i < 8; j++)			
				novo->taboo[i][j] = last->taboo[i][j];		
		novo->anterior = NULL;
		novo->seguinte = NULL;
		return novo;
	}
	else
	{
		while (board->seguinte != NULL)
		{
			board = board->seguinte;		
		}
		for (i = 0; i < 8; i++)		
			for (j = 0; j < 8; j++)			
				novo->taboo[i][j] = last->taboo[i][j];					
		novo->anterior = board;
		novo->seguinte = NULL;
		board->seguinte = novo;
		return novo;
	}
}

int loadGameFile(tab board, int *idJog, int *r1, int *r2,int *nJog)
{
	fseek(stdin, 0, SEEK_END);
	int i, j, stop = 0, nj;
	char str[100];
	char *fileName;
	char chAux[5] = { 'p','0','b','B','P'}; //Tamanho temporário
	int op = 0, bk = 0;
	do
	{
		system("cls");
		puts("Insira um nome para o ficheiro : ");
		scanf("%s", str);
		fileName = strcat(str, ".bin");
		if (existe(str) == 1)
		{
			puts("FICHEIRO EXISTE");
			FILE *newfile;
			newfile = fopen(fileName, "rb");
			if (feof(newfile))
			{
				stop = 1;
				break;
			}
			else
			{				
				fread(&nj, sizeof(int), 1, newfile);
				*nJog = nj;
			}			
			tab tabu2 = (tab)malloc(sizeof(struct tabuleiro));
			tabu2->anterior = NULL;
			tabu2->seguinte = NULL;
			tab tabu = (tab)malloc(sizeof(struct tabuleiro));
			tabu->anterior = NULL;
			tabu->seguinte = NULL;
			int count = 0;
			do
			{								
				for (i = 0; i < 8; i++)
				{
					for (j = 0; j < 8; j++)
					{
						int cAux = 0;
						if (feof(newfile))
						{
							stop = 1;
							break;
						}
						else
						{
							fread(&cAux, sizeof(int), 1, newfile);
						}
						if (cAux >= 0 || cAux <= 5)
						{
							tabu->taboo[i][j] = chAux[cAux];
						}
						else
						{							
							stop = 1;
							break;
						}
					}
				}						
				tabu2 = inserirFim(tabu2, tabu);				
				drawBoard(tabu2,0);
				count++;
			} while (count < nj);			
			/*MapaInicio(tabu);
			tabu2 = inserirFim(tabu2,tabu);*/			
			if (stop == 0)
			{				
				*board = *tabu2;
				int id, rr, rrr;	
				if (feof(newfile))
				{
					stop = 1;
					fclose(newfile);
					break;
				}
				else			
					fread(&id, sizeof(int), 1, newfile);						
				if (feof(newfile))
				{
					stop = 1;
					fclose(newfile);
					break;
				}
				else				
					fread(&rr, sizeof(int), 1, newfile);							
				if (feof(newfile))
				{			
					stop = 1;
					fclose(newfile);
					break;
				}
				else				
					fread(&rrr, sizeof(int), 1, newfile);							
				if (((id >= 0 && id <= 1 && id != NULL) ||
					(rr <= 3 && rr >= 0 && rr != NULL) ||
					(rrr <= 3 && rrr >= 0 && rrr != NULL)) && stop == 0)
				{
					*idJog = id;
					*r1 = rr;
					*r2 = rrr;
					fclose(newfile);
					while (getchar() != '\n');
					getchar();
					system("cls");
					stop = 0;
					break;
				}
				else { stop = 1; }
			}
			if (stop == 1)
			{
				system("cls");
				int op;
				puts("FICHEIRO NAO VALIDO !!\nComo o ficheiro nao e valido vai ser Eliminado !!\n1-Tentar de novo\n2-Comecar novo jogo");
				remove(fileName);
				scanf("%d", &op);
				if (op == 2)
				{
					system("cls");
					return 1;
				}
				else bk = 1;
			}
		}
		else
		{
			system("cls");
			int op;
			puts("Ficheiro nao existe !!\n1-Tentar de novo\n2-Comecar novo jogo");
			scanf("%d", &op);
			if (op == 2)
			{
				system("cls");
				return 1;
			}
			else bk = 1;
		}
		stop = 0;
		system("cls");
	} while (bk == 1);
	return 0;
}

int MENU()
{
	system("cls");
	puts("BEM VINDO AO JOGO DAS DAMAS !!");
	puts("\nPrecione : \n1-Novo Jogo\n2-Continuar Jogo");
	int op;
	scanf("%d", &op);
	system("cls");
	return op;
}

//int Vitoria(tab board, char jog)
//{
//	int x, y;
//	tabarv tb;
//
//	for (x = 0; x < 8; x++)
//	{
//		for (y = 0; y<8; y++)
//		{
//			tb = jogadas(board, jog, x, y, 0);
//			if (movivel() == 1) return 0;
//		}
//	}
//	return 1;
//}

int main()
{	
	int jogId = 0,nJogadas = 0,semP = -1;
	int retr[2] = {3,3};
	char charsPoss[2][2] = { { 'b','B' },
						     { 'p','P' } };
	char opChar;
	int i = 0,first = 1,repeat = 1;

	tab tabu = (tab)malloc(sizeof(struct tabuleiro));
	tabu->anterior = NULL;
	tabu->seguinte = NULL;

	while (repeat == 1)
	{
		int opMenu = MENU();
		if (opMenu == 1)
		{
			MapaInicio(tabu);
			tabu = saveLastBoard(tabu);
			first = 0;
		}
		else
		{
			int a = loadGameFile(tabu, &jogId, &retr[0], &retr[1], &nJogadas);
			first = 1;
			if (a == 1)
			{
				first = 0;
				MapaInicio(tabu);
				tabu = saveLastBoard(tabu);
			}
		}
		while (semP == -1)
		{
			fseek(stdin, 0, SEEK_END);
			fflush(stdin);
			system("cls");
			semP = semPecas(tabu);
			if (semP == 0)
			{
				jogId = 0;
				break;
			}
			else if (semP == 1)
			{
				jogId = 1;
				break;
			}
			tabu = saveLastBoard(tabu);
			printf("\nJogador %d (%c/%c) e a sua vez.\n", jogId + 1, charsPoss[jogId][0], charsPoss[jogId][1]);
			if (first == 1)
			{
				printf("\nQuer anular a jogada anterior do adversario?(s/S)\n");
				scanf("%c", &opChar);
				if (opChar == 's' || opChar == 'S')
				{
					if (retr[jogId]>0)
					{
						retrocederJogada(tabu);
						retr[jogId]-= 1;
						puts("Conluido.\nContinue a sua jogada");
						fflush(stdin);
						getchar();
						system("cls");
					}
					else
						puts("Já nao pode anular mais jogadas ...");
				}
				system("cls");
			}
			int x, y, repeat2 = 0;
			system("cls");
			do
			{						
				printf("\nJogador %d (%c/%c) e a sua vez.\n", jogId + 1, charsPoss[jogId][0], charsPoss[jogId][1]);
				drawBoard(tabu,0);
				printf("\nIndique as coordenadas da peca que quer jogar\n");
				printf("Linha : ");
				scanf("%d", &x);
				printf("\nColuna : ");
				scanf("%d", &y);
				if (tabu->taboo[x][y] != charsPoss[jogId][0] && tabu->taboo[x][y] != charsPoss[jogId][1])
				{
					puts("Nao possui essa peca...\nTente novamente...");
					printf("\nCasa Escolhida -> %c\n", tabu->taboo[x][y]);
					printf("Casas do jogador -> %c || %c\n", charsPoss[jogId][0], charsPoss[jogId][1]);
				}
				else
				{
					tabarv tb = jogadas(tabu, tabu->taboo[x][y], x, y, 0, 0);
					if (tb->AntDir == NULL && tb->AntEsq == NULL && tb->SegDir == NULL && tb->SegEsq == NULL)
					{
						puts("Esta peca nao e valida !!\nTente novamente...");
						repeat2 = 1;
					}
				}
			} while (tabu->taboo[x][y] != charsPoss[jogId][0] && tabu->taboo[x][y] != charsPoss[jogId][1] || repeat2 == 1);

			tabarv tb = jogadas(tabu, tabu->taboo[x][y], x, y, 0, 0);
			escolheCaminho(tb);
			moverPeca(tabu, tb, tabu->taboo[x][y]);
			drawBoard(tabu,0);
			fseek(stdin, 0, SEEK_END);
			fflush(stdin);						
			printf("\n1-Salvar o jogo num ficheiro\n2-Ver todas as jogadas anteriores\n-1-Render\n4-Continuar\n");
			int opcao;
			scanf("%d", &opcao);
			nJogadas++;
			if (opcao == 1)
			{
				saveGameFile(tabu, jogId, retr[0], retr[1], nJogadas);
			}
			else if (opcao == 2)
			{
				drawBoard(tabu, 1);
			}
			else if (opcao == -1)
			{
				if (jogId == 0)
					jogId = 1;
				else jogId = 0;
				break;
			}
			if (jogId == 0)
				jogId = 1;
			else jogId = 0;
			while (getchar() != '\n');
			getchar();
		}
		fseek(stdin, 0, SEEK_END);
		fflush(stdin);
		system("cls");
		printf("Parabens jogador %d e o vencedor !!!", jogId+1);
		printf("\n\nQuer jogar novamente ?(s/S)\n");		
		scanf("%c", &opChar);
		if (opChar == 's'|| opChar == 'S')		
			repeat = 1;		
		else		
			repeat = 0;	
	}	
	while (getchar() != '\n');
	getchar();
}