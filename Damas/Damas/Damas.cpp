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
	struct tabuleiro *segunte;
}*tab;

//typedef struct Lista
//{
//	struct Lista *anterior;
//	struct Lista *seguinte;
//	struct jogada *jog;
//}*ListJogs;

typedef struct JogsPoss
{
	struct jogada *esquerda;
	struct jogada *direita;
	int casa[2];
}*jogada;	

//tab saveLastBoard(tab board)
//{
//
//}
//
//tab retroceder(tab board)
//{
//	
//}
 
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
	if (come == 5)
	{
		return NULL;
	}
	else if ((board->taboo[linha][coluna] == '0') && (come == 0))
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
			tb->SegDir = NULL;
			tb->SegEsq = NULL;
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
					tb->AntEsq = jogadas(board, jog, linha - 2, coluna - 2, come + 1);
					tb->AntEsq->SegDir = tb;
				}
				else
				{
					tb->AntEsq = NULL;
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
			else tb->AntDir = NULL;
		}
		//Jogador preto
		if (jog == 'p')
		{
			tb->AntDir = NULL;
			tb->AntEsq = NULL;
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
					tb->SegEsq->AntDir = tb;
				}

				else tb->SegEsq = NULL;
			}
			//Direita
			if ((linha + 1 > 7) || (coluna + 1 > 7) || (board->taboo[linha + 1][coluna + 1] == 'p')) tb->SegDir = NULL;
			else
			{
				if ((come == 0) && (board->taboo[linha + 1][coluna + 1] == '0'))
				{
					tb->SegDir = (tabarv)malloc(sizeof(struct TabArv));
					tb->SegDir->AntDir = NULL;
					tb->SegDir->SegDir = NULL;
					tb->SegDir->SegEsq = NULL;
					tb->SegDir->AntEsq = tb;
					tb->SegDir->casa[0] = linha + 1;
					tb->SegDir->casa[1] = coluna + 1;
					tb->SegDir->comestivel = 0;
				}
				else if ((board->taboo[linha + 1][coluna + 1] == 'b') && (linha + 2 >= 0) && (coluna + 2 < 8) && (board->taboo[linha + 2][coluna + 2] == '0'))
				{
					tb->SegDir = jogadas(board, jog, linha + 2, coluna + 2, come + 1);
					tb->SegDir->AntEsq = tb;
				}
				else tb->SegDir = NULL;
			}
		}
		return tb;
	}
}

//verificar se nos movimentos possiveis existe algum comer possivel
int maxComestivel(tabarv tb)
{
	if ((tb->AntDir != NULL) && (tb->AntDir->comestivel > tb->comestivel)) return tb->AntDir->comestivel;
	if ((tb->AntEsq != NULL) && (tb->AntEsq->comestivel > tb->comestivel)) return tb->AntEsq->comestivel;
	if ((tb->SegDir != NULL) && (tb->SegDir->comestivel > tb->comestivel)) return tb->SegDir->comestivel;
	if ((tb->SegEsq != NULL) && (tb->SegEsq->comestivel > tb->comestivel)) return tb->SegEsq->comestivel;

	return tb->comestivel;
}


//print dos caminhos possiveis
void caminhos(tabarv tb)
{
	if (tb != NULL)
	{
		int comes = maxComestivel(tb);

		if (tb->AntEsq != NULL && tb->AntEsq->comestivel >=comes)
			caminhos(tb->AntEsq);
		if (tb->AntDir != NULL && tb->AntDir->comestivel >=comes)
			caminhos(tb->AntDir);

		int i;
		for (i = 0; i < tb->comestivel; i++) printf("\t");			//tabs para ficar agradavel à vista as jogadas possiveis
		printf("[%d,%d]\n", tb->casa[0], tb->casa[1]);
		
		if (tb->SegEsq != NULL && tb->SegEsq->comestivel >=comes)
			caminhos(tb->SegEsq);
		if (tb->SegDir != NULL && tb->SegDir->comestivel >=comes)
			caminhos(tb->SegDir);
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
void MapaInicio(tab board)
{
	char b[8][8] = {
	{ 'p', '0', 'p', '0', 'p', '0', 'p', '0' },
	{ '0', 'p', '0', 'p', '0', 'p', '0', 'p' },
	{ 'p', '0', 'p', '0', 'p', '0', 'p', '0' },
	{ '0', '0', '0', 'b', '0', '0', '0', '0' },
	{ '0', '0', '0', '0', '0', '0', '0', '0' },
	{ 'b', '0', 'b', 'b', 'b', 'b', 'b', '0' },
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
int existe(char *fname)
{
	FILE *file;

	char* name = concat(fname, ".bin");

	if (file = fopen(name, "rb"))
	{
		fclose(file);
		return 1;
	}
	return 0;
}
void saveGameFile(tab board, int idJog, int r1, int r2)
{
	fseek(stdin, 0, SEEK_END);
	int i, j;
	char str[100];	
	int op = 0, bk = 0;
	do
	{
		fseek(stdin, 0, SEEK_END);
		system("cls");
		puts("Insira um nome para o ficheiro : ");
		scanf("%[^\n]s", str);
		system("cls");
		if (existe(str) == 1)
		{
			puts("Ficheiro ja existe...\n-1 Substituir\n-2 Inserir novo nome\n-3 Nao salvar");
			scanf("%d", &op);
			if (op == 1)
			{
				char * fileName = concat(str, ".bin");
				FILE *newfile;
				newfile = fopen(fileName, "wb");
				for (i = 0; i < 8; i++)
				{
					for (j = 0; j < 8; j++)
					{
						char cAux = board->taboo[i][j];
						fwrite(&cAux, sizeof(char), 1, newfile);
						//fprintf(newfile, "%c\n", cAux);//.txt
					}
				}
				//fprintf(newfile, "%d\n", idJog);//.txt
				fwrite(&idJog, sizeof(int), 1, newfile);
				fwrite(&r1, sizeof(int), 1, newfile);
				fwrite(&r2, sizeof(int), 1, newfile);
				fclose(newfile);
				puts("Save efetuado com sucesso !!");
				while (getchar() != '\n');
				getchar();
				break;
			}
			else if (op == 2)
			{
				bk = 1;
			}
			else if (op == 3)
			{
				puts("Terminado !!");
				while (getchar() != '\n');
				getchar();
				break;
			}
		}
		else
		{
			char * fileName = concat(str, ".bin");
			FILE *newfile;
			newfile = fopen(fileName, "wb");
			for (i = 0; i < 8; i++)
			{
				//for (int j = 0; j < 8; j++)
				//{
				//	char cAux = board->taboo[i][j];
				//	fwrite(&cAux, sizeof(char), 1, newfile);
				//	//fprintf(newfile, "%c\n", cAux); //.txt
				//}

				for (j = 0; j < 8; j++)
				{
					char cAux = board->taboo[i][j];
					int iAux;
					if (cAux == 'p')
						iAux = 0;
					else if (cAux == '0')
						iAux = 1;
					else if (cAux == 'b')
						iAux = 2;
					else iAux = 200;
					fwrite(&iAux, sizeof(int), 1, newfile); //.bin
				}
			}			
			fwrite(&idJog, sizeof(int), 1, newfile);//binario
			fwrite(&r1, sizeof(int), 1, newfile);
			fwrite(&r2, sizeof(int), 1, newfile);
			fclose(newfile);
			puts("Save efetuado com sucesso !!");
			while (getchar() != '\n');
			getchar();
			break;
		}
	} while (bk == 1);
}
int loadGameFile(tab board, int *idJog, int *r1, int *r2)
{
	fseek(stdin, 0, SEEK_END);
	int i, j,stop = 0;
	char str[100];
	char chAux[3] = { 'p','0','b'}; //Tamanho temporário
	int op = 0, bk = 0;
	do
	{
		system("cls");
		puts("Insira um nome para o ficheiro : ");
		scanf("%s", str);
		if (existe(str) == 1)
		{
			puts("FICHEIRO EXISTE");
			char * fileName = concat(str, ".bin");
			FILE *newfile;
			newfile = fopen(fileName, "rb");
			for (i = 0; i < 8; i++)
			{
				//for (int j = 0; j < 8; j++)
				//{
				//	char cAux;
				//	fread(&cAux, sizeof(char), 1, newfile);//binario
				//										   
				//	board->taboo[i][j] = cAux;
				//}
				for (j = 0; j < 8; j++)
				{
					int cAux;
					fread(&cAux, sizeof(int), 1, newfile);//binario
					
					if (cAux == 0 || cAux == 1 || cAux == 2 || cAux == 3)
					{
						board->taboo[i][j] = chAux[cAux];
					}
					else
					{
						j = 8;
						i = 8;
						stop = 1;
					}					
				}								
			}
			if (stop == 0)
			{
				int id, rr, rrr;
				fread(&id, sizeof(int), 1, newfile);//binario
				fread(&rr, sizeof(int), 1, newfile);
				fread(&rrr, sizeof(int), 1, newfile);
				if ((id >= 1 && id <= 2) ||
					(rr <= 3 && rr >= 0) ||
					(rrr <= 3 && rrr >= 0))
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
				else { stop = 1;}
				
			}

			if (stop == 1)
			{			
				system("cls");
				int op;
				puts("FICHEIRO NAO VALIDO !!\n1-Tentar de novo\n2-Comecar novo jogo");
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
	puts("Precione : \n1-Novo Jogo\n2-Continuar Jogo");
	int op;
	scanf("%d", &op);
	system("cls");
	return op;
}
int main()
{	
	jog jogs[2];
	int jogId = 1, retr1 = 2, retr2 = 3;
	char aux[2] = { 'p','b' };
	int i;
	for (i = 0; i < 2; i++)
	{
		jogs[i] = (jog)malloc(sizeof(struct jogador));
		jogs[i]->retroceder = 3;
		jogs[i]->peca = aux[i];
	}	
	tab tabu = (tab)malloc(sizeof(struct tabuleiro));

	MapaInicio(tabu);
	/*tabarv tb = jogadas(tabu, 'b', 6, 5, 0);
	caminhos(tb);	*/
	//tabu->taboo[2][2] = 'a';
	/*saveGameFile(tabu, jogId, retr1, retr2);
	
	tabu->taboo[2][2] = 'a';
	int opMenu = MENU();
	if (opMenu == 1)
	{
		MapaInicio(tabu);
	}
	else
	{
		int a = loadGameFile(tabu, &jogId, &retr1, &retr2);
		if (a == 1)
		{
			MapaInicio(tabu);
		}
	}
*/
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

	/*printf("Existe - %d\nJogId - %d\nJog1 retr - %d\nJog2 retr - %d", existe("a"), jogId, retr1, retr2);*/
	while (getchar() != '\n');
	getchar();
}