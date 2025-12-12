Trabalho de Fundamentos da Programação

Grupo de Trabalho 
Afonso Raimundo 20251105
António Silva 20250370
Tomas Estrela 20210282
Miguel Carvalho 20220881

Estrutura do Projeto

projeto/ README.md src/ Program.cs Model/ Player.cs Space.cs Board.cs Game.cs Card.cs View/ MainView.cs ConsoleView.cs Controller/ GameController.cs CommandHandler.cs

README:
O ficheiro README.md contém o relatório sobre o projeto, os membros do grupo, e a distribuição de tarefas

Program.cs
O ficheiro Program.cs é o ponto de entrada da aplicação.
É responsável por:
Inicializar o jogo
Criar as instâncias principais (controller e views)
Iniciar o ciclo principal do programa
Este ficheiro não contém lógica do jogo, apenas coordena o arranque da aplicação.

Model
A Pasta Model contém todas as classes que representam os dados e o estado do jogo.

Player.cs
Representa um jogador do jogo contendo informação como:

Space.cs
Representa um estado do tabuleiro, como:

Board.cs 
Responsável pela criação e gestão do tabuleiro do jogo.

Game.cs
Classe central da lógica do jogo.

Card.cs
Representa as Cartas do jogo(Chance e Community)

VIEW
A Pasta View é Responsável pela interação com o utilizador através da consola.

MainView.cs
Define o fluxo principal pela interação com o utilizador através da consola.
É responsável por:
-Ler inputs do utilizador
-Controlar o ciclo de execução do Jogo

ConsoleView.cs
Responsável pela apresentação de informação na consola, como:
- Mostrar tabuleiro
- Mostrar estado do Jogo 
- Apresentar detalhes dos jogadores

CONTROLADORES
A Pasta Controller faz a ligação entre as pastas View e o model.

GameController.cs
Responsável por executar a lógica do jogo com base nos pedidos feitos pela View como:
- Registar Jogadores
- Listar Jogadores
- Iniciar Jogo
- Execução de comandos do jogo

Distribuição de tarefas:

Afonso — RJ, LJ, Tabuleiro, Gestão de jogadores e Troubleshooting/Testes ao programa. António — LD, PA, TT Movimentos, dados, turnos e eventos imediatos. Tomás — CE, CC Compras de espaços e casas e estado base do jogo. Miguel — TC, DJ, IJ Cartas, efeitos especiais e renderização do tabuleiro


Execução do projeto
dotnet run