using System;

namespace CatAndMouse
{
    enum GameState
    {
        Start,  
        End     
    }

    class Game
    {
        public int size;            
        public Player cat;
        public Player mouse;
        public GameState state;

        public Game(int size)
        {
            this.size = size;
            cat = new Player("Cat", size);
            mouse = new Player("Mouse", size);
            state = GameState.Start;
        }

        public void DoMoveCommand(char command, int steps)
        {
            switch (command)
            {
                case 'M': mouse.Move(steps); break;
                case 'C': cat.Move(steps); break;
            }
        }

        public int GetDistance()
        {
            return Math.Abs(cat.location - mouse.location);
        }

        public bool IsMouseCaught()
        {
            return cat.state != State.NotInGame
                && mouse.state != State.NotInGame
                && cat.location == mouse.location;
        }
    }
}