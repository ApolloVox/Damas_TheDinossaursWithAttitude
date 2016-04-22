// Damas.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
	

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
	int casa[2]; //0->vazia, 1->jog1, 2->jog2
	int comestivel;
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

ultimoTab saveLastBoard(ultimoTab last ,tab board)
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

tabarv jogadas(tab board, char jog, int linha, int coluna, int come) //se comeu uma peça 1, se não comeu 0
{
	if ((board->taboo[linha][coluna] == '0') && (come == 0))
	{
		printf("Casa nao e jogavel\n");
		return NULL;
	}
	else if ((jog != board->taboo[linha][coluna]) && (come == 0))
	{
		printf("Peca nao e do jogador.\n");
		return NULL;
	}
	else
	{
		tabarv tb = (tabarv)malloc(sizeof(struct TabArv));
		tb->casa[0] = linha;
		tb->casa[1] = coluna;
		tb->comestivel = come;
		//Jogador Branco
		if (jog == 'b')
		{
			//Esquerda
			if ((linha - 1 < 0) || (coluna - 1 < 0) || (board->taboo[linha - 1][coluna - 1] == 'b'))
			{
				tb->AntEsq = NULL;
			}
			else
			{
				if ((come == 0) && (board->taboo[linha - 1][coluna - 1] == '0'))
				{
					tb->AntEsq = (tabarv)malloc(sizeof(struct TabArv));
					tb->AntEsq->AntDir = NULL;
					tb->AntEsq->SegDir = tb;
					tb->AntEsq->SegEsq = NULL;
					tb->AntEsq->AntEsq = NULL;
					tb->AntEsq->casa[0] = linha - 1;
					tb->AntEsq->casa[1] = coluna - 1;
					tb->AntEsq->comestivel = 0;
				}

				else if ((board->taboo[linha - 1][coluna - 1] == 'p') && (linha - 2 >= 0) && (coluna - 2 >= 0) && (board->taboo[linha - 2][coluna - 2] == '0'))
				{
					tb->AntEsq=jogadas(board, jog, linha - 2, coluna - 2, come + 1);
					tb->AntEsq->SegDir = tb;
				}
			}
			//Direita
			if ((linha - 1 > 7) || (coluna + 1 < 0) || (board->taboo[linha - 1][coluna + 1] == 'b'))
			{
				tb->AntDir = NULL;
			}

			else if ((come == 0) && (board->taboo[linha - 1][coluna + 1] == '0'))
			{
				tb->AntDir = (tabarv)malloc(sizeof(struct TabArv));
				tb->AntDir->AntDir = NULL;
				tb->AntDir->SegEsq = tb;
				tb->AntDir->SegDir = NULL;
				tb->AntDir->AntEsq = NULL;
				tb->AntDir->casa[0] = linha - 1;
				tb->AntDir->casa[1] = coluna + 1;
				tb->AntDir->comestivel = 0;
			}
			else if ((board->taboo[linha - 1][coluna + 1] == 'p') && (linha - 2 < 8) && (coluna + 2 >= 0) && (board->taboo[linha - 2][coluna + 2] == '0'))
			{
				tb->AntDir = jogadas(board, jog, linha - 2, coluna + 2, come + 1);
				tb->AntDir->SegEsq = tb;
			}
		}
		//Jogador preto
		if (jog == 'p')
		{
			//Esquerda
			if ((linha + 1 < 0) || (coluna - 1 > 7) || (board->taboo[linha + 1][coluna - 1] == 'p')) tb->SegEsq = NULL;
			else
			{
				if ((come == 0) && (board->taboo[linha + 1][coluna - 1] == '0'))
				{
					tb->SegEsq = (tabarv)malloc(sizeof(struct TabArv));
					tb->SegEsq->AntDir = tb;
					tb->SegEsq->SegDir = NULL;
					tb->SegEsq->SegEsq = NULL;
					tb->SegEsq->AntEsq = NULL;
					tb->SegEsq->casa[0] = linha + 1;
					tb->SegEsq->casa[1] = coluna - 1;
					tb->SegEsq->comestivel = 0;
				}

				else if ((board->taboo[linha + 1][coluna - 1] == 'b') && (linha + 2 >= 0) && (coluna - 2 < 8) && (board->taboo[linha + 2][coluna - 2] == '0'))
				{
					tb->SegEsq = jogadas(board, jog, linha + 2, coluna - 2, come + 1);
					tb->SegEsq->AntDir= tb;
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
						jogadas(board, jog, linha + 2, coluna + 2,tb, come+1);
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
	{ '0', 'p', '0', '0', '0', '0', '0', '0' },
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
}

char* concat(char *s1, char *s2)
{
	char *result = malloc(strlen(s1) + strlen(s2) + 1);
													   
	strcpy(result, s1);
	strcat(result, s2);
	return result;
}
int exists(char *fname)
{
	FILE *file;	

	char* name = concat(fname,".bin");

	if (file = fopen(name, "rb"))
	{
		fclose(file);
		return 1;
	}
	return 0;
}
void saveGameFile(tab board,int idJog)
{
	fseek(stdin, 0, SEEK_END);
	char str[100];
	int op = 0,bk = 0;	
	do
	{
		system("cls");
		puts("Insira um nome para o ficheiro : ");
		scanf("%[^\n]%*c", str);
		system("cls");
		if (exists(str) == 1)
		{
			puts("Ficheiro ja existe...\n-1 Substituir\n-2 Inserir novo nome\n-3 Nao salvar");
			scanf("%d", &op);
			if (op == 1)
			{
				char * fileName = concat(str, ".bin");
				FILE *newfile;
				newfile = fopen(fileName, "wb");
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						char cAux = board->taboo[i][j];
						fwrite(&cAux, sizeof(char), 1, newfile);
						//fprintf(newfile, "%c\n", cAux);
					}
				}
				//fprintf(newfile, "%d\n", idJog);
				fwrite(&idJog, sizeof(int), 1, newfile);
				fclose(newfile);
				puts("Save efetuado com sucesso !!");
				getchar();
				break;
			}
			else if(op == 3)
			{
				puts("Terminado !!");
				getchar();				
				break;
			}
			else
			{
				bk = 1;
			}
		}
		else
		{
			char * fileName = concat(str, ".bin");
			FILE *newfile;
			newfile = fopen(fileName, "wb");
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					char cAux = board->taboo[i][j];
					fwrite(&cAux, sizeof(char), 1, newfile);
					//fprintf(newfile, "%c\n", cAux); //.txt
				}
			}
			//fprintf(newfile, "%d\n", idJog);//.txt
			fwrite(&idJog, sizeof(int), 1, newfile);//binario
			fclose(newfile);
			puts("Save efetuado com sucesso !!");
			getchar();
			break;
		}
		getchar();
	} while (exists(str) == 1 || bk == 1);	
}
int loadGameFile(tab board,int idJog)
{
	fseek(stdin, 0, SEEK_END);
	char str[100];
	int op = 0, bk = 0;
	do
	{
		system("cls");
		puts("Insira um nome para o ficheiro : ");
		scanf("%[^\n]%*c", str);			
		if (exists(str) == 1)
		{
			puts("FICHEIRO EXISTE");
			char * fileName = concat(str, ".bin");
			FILE *newfile;
			newfile = fopen(fileName, "rb");
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					char cAux2;
					fread(&cAux2, sizeof(char), 1, newfile);//binario
					//fscanf(newfile, "%c\n", &cAux2);//.txt
					board->taboo[i][j] = cAux2;
				}
			}
			fread(&idJog, sizeof(int), 1, newfile);//binario
			//fscanf(newfile, "%d\n", idJog);//it Works - //.txt
			fclose(newfile);
			getchar();
			break;
		}
		else
		{
			int op;
			puts("Ficheiro nao existe !!\n1-Tentar de novo\n2-Comecar novo jogo");
			scanf("%d", &op);
			if (op == 2)
			{
				return 1;
				bk = 1;
			}			
		}

		getchar();
	} while (exists(str) == 0 || bk == 1);
	return 0;
}

int MENU()
{
	system("cls");
	puts("BEM VINDO AO JOGO DAS DAMAS !!");
	puts("Precione : \n1-Continuar Jogo\n2-Novo Jogo");
	int op;
	scanf("%d",&op);
	system("cls");
	return op;
}
int main()
{	
	FILE *save;

	jog jogs[2];
	int jogId = 123;
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
	tabu->taboo[2][2] = 'v';
	saveGameFile(tabu, jogId);
	tabu->taboo[2][2] = 'z';		
	int opMenu = MENU();	
	if (opMenu == 2)
	{
		MapaInicio(tabu);		
	}
	else
	{
		int a = loadGameFile(tabu,&jogId);
		if (a == 1)
		{
			MapaInicio(tabu);
		}
	}	

	/*drawBoard(tabu);*/
	/*lt = saveLast(lt, tabu);
	drawBoard2(lt);
	tabu->taboo[2][2] = 'b';
	drawBoard(tabu);
	tabu = retroceder(lt, tabu);
	int coord[2];*/
	/*printf("Indique as coordenadas da peça que quer jogar\n");				
	
	drawBoard(tabu);	
	jogadas(tabu, 'b', 7,7,tb,0);*/

	drawBoard(tabu);
	
	printf("Existe - %d\nJogId - %d", exists("yolo"),jogId);
	getchar();	
}