using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace Game1
{
    /// <summary>
    /// Основной класс нашей игры.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        //SpriteBatch - класс, позволяющий отрисовывать спрайты
        SpriteBatch spriteBatch;
        SpriteFont spritefont;

        //Объявим глобальную переменную типа Texture2D для хранения информации о нашей текстуре.
        Texture2D big_field;
        Texture2D food;
        Texture2D squirrel;
        /// <summary>
        /// Snake textures
        /// </summary>
        Texture2D snake_head;
        Texture2D snake_wow;
        Texture2D snake_nyah;
        Texture2D snake_body_even;
        Texture2D snake_body_odd;
        Texture2D snake_turn;
        Texture2D snake_tail1;
        Texture2D snake_tail2;      
        /// <summary>
        /// Menu textures
        /// </summary>
        Texture2D play;
        Texture2D options;
        Texture2D difficulty;
        Texture2D hard;
        Texture2D easy;
        Texture2D normal;
        Texture2D sound;
        Texture2D on;
        Texture2D off;
        Texture2D play_pressed;
        Texture2D options_pressed;
        Texture2D difficulty_pressed;
        Texture2D hard_pressed;
        Texture2D easy_pressed;
        Texture2D normal_pressed;
        Texture2D sound_pressed;
        Texture2D on_pressed;
        Texture2D off_pressed;
        Texture2D back;
        Texture2D back_pressed;
        /// -------------------
        Rectangle play_rectangle;
        Rectangle options_rectangle;
        Rectangle difficulty_rectangle;
        Rectangle hard_rectangle;
        Rectangle easy_rectangle;
        Rectangle normal_rectangle;
        Rectangle sound_rectangle;
        Rectangle on_rectangle;
        Rectangle off_rectangle;
        Rectangle options_back;
        Rectangle difficulty_back;
        Rectangle sound_back;
        /// -------------------
        SoundEffect menuSong;
        SoundEffect intro;
        SoundEffect loop;
        SoundEffect death;
        SoundEffect spiderintro;
        SoundEffect spiderloop1;
        SoundEffect spiderloop2;

        SoundEffectInstance menuInstance;
        SoundEffectInstance introInstance;
        SoundEffectInstance loopInstance;
        SoundEffectInstance deathInstance;
        SoundEffectInstance spiderloop1Instance;
        SoundEffectInstance spiderloop2Instance;


        /// -------------------
        public enum GameStates
        {
            Menu,
            Playing,
            Dead,
            Menu_difficulty,
            Menu_sound,
            Menu_options
                
        }
        public enum difficulties
        {
            easy,
            hard,
            normal,
       
        }
        KeyboardState currentstate;
        KeyboardState oldstate;
        MouseState curmouse;
        MouseState oldmouse;

        public GameStates _gameState = GameStates.Menu;
        public difficulties _dif = difficulties.normal;
        int skin = 1;
        bool skinrand;
        
            
        int divide_speed = 10;
        int score;
        int delta = 0;
        int nyah = 0; //nyahing frames
        int tail_oddity = 0;
        int[,] field;
        int x_head, y_head;// голова змеи
        int x_food, y_food;
        int x_squirrel=-13, y_squirrel=-13;
        int squirrel_countdown;
        int squirrel_count=0;
        int move_direction; // 1 - up, 2 - down, 3 - right, 4 - left, visible direction
        int temp_move_direction; //changing direction. is written to move_direction before field update
        float rotation;
        int length;
        float scale = 2f;
        int window_width = (20 * 18 ) * 2;

        int window_height = (20 * 18 ) * 2;
        bool menumusic = false;
        bool intromusic = false;
        bool deathmusic = false;
        bool soundison = true;
       
        
     

        Random rand = new Random();
     //   private bool gui_rectangle_checkmouse(Rectangle rct)
        //{
        //    if (rct.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
        //    {
        //        return true;

        //    }
        //    else return false;
        //}
      
        private void init_field()
        {
            big_field = Content.Load<Texture2D>("Terrain1-1");
        }
        private void init_logic_field()
        {
            field = new int[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    field[i, j] = 0;
                }
            }
            score = 0;

        }
        private void init_snake()
        {
            skin = rand.Next(1, 17);
            if (skin == 17)
            {
                skin = 1;
                skinrand = true;
            }

            x_head = rand.Next(2, 17);
            y_head = rand.Next(2, 17);
            field[x_head, y_head] = 1;
            field[x_head, y_head + 1] = 2;
            field[x_head, y_head + 2] = 3;
            temp_move_direction = 1;
            length = 3;
            rotation = 0f;
            snake_head = Content.Load<Texture2D>("snakehead");
            snake_wow = Content.Load<Texture2D>("snakewow");
            snake_nyah = Content.Load<Texture2D>("snakenyah");
            snake_body_even = Content.Load<Texture2D>("snakebody1");
            snake_body_odd = Content.Load<Texture2D>("snakebody1");
            snake_tail1 = Content.Load<Texture2D>("Snaketail1");
            snake_tail2 = Content.Load<Texture2D>("Snaketail2");
            snake_turn = Content.Load<Texture2D>("Snaketurn");
            spritefont = Content.Load<SpriteFont>("font1");
        }
        private void init_food()
        {
            do
            {
                x_food = rand.Next(20);
                y_food = rand.Next(20);
            } while (field[y_food, x_food] > 0);
            food = Content.Load<Texture2D>("Apple");
            squirrel = Content.Load<Texture2D>("squirrel");

        }

        private void init_menu()
        {
            play = Content.Load<Texture2D>("MenuPlay");
            options = Content.Load<Texture2D>("MenuOptions");
            difficulty = Content.Load<Texture2D>("MenuDifficulty");
            hard = Content.Load<Texture2D>("MenuHard");
            easy = Content.Load<Texture2D>("MenuEasy");
            normal = Content.Load<Texture2D>("MenuNormal");
            sound = Content.Load<Texture2D>("MenuSound");
            on = Content.Load<Texture2D>("MenuOn");
            off = Content.Load<Texture2D>("MenuOff");
            play_pressed = Content.Load<Texture2D>("MenuPlayPressed");
            options_pressed = Content.Load<Texture2D>("MenuOptionsPressed");
            difficulty_pressed = Content.Load<Texture2D>("MenuDifficultyPressed");
            hard_pressed = Content.Load<Texture2D>("MenuHardPressed");
            easy_pressed = Content.Load<Texture2D>("MenuEasyPressed");
            normal_pressed = Content.Load<Texture2D>("MenuNormalPressed");
            sound_pressed = Content.Load<Texture2D>("MenuSoundPressed");
            on_pressed = Content.Load<Texture2D>("MenuOnPressed");
            off_pressed = Content.Load<Texture2D>("MenuOffPressed");
            back = Content.Load<Texture2D>("back");
            back_pressed = Content.Load<Texture2D>("back_pressed");

            ///-------------------------------
            play_rectangle = new Rectangle(window_width / 2- play.Width, window_height / 3-play.Height, play.Width*2,play.Height*2);
            options_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 3*2 - options.Height, options.Width*2, options.Height*2);
            difficulty_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 4 * 2 - options.Height, options.Width * 2, options.Height * 2);
            sound_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 4 * 3 - options.Height, options.Width * 2, options.Height * 2);
            options_back = new Rectangle(window_width / 2 - options.Width, window_height / 4  - options.Height, options.Width * 2, options.Height * 2);
            difficulty_back = new Rectangle(window_width / 2 - options.Width, window_height / 5 - options.Height, options.Width * 2, options.Height * 2);
            hard_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 5*4 - options.Height, options.Width * 2, options.Height * 2);
            normal_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 5*3 - options.Height, options.Width * 2, options.Height * 2);
            easy_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 5*2 - options.Height, options.Width * 2, options.Height * 2);
            sound_back = new Rectangle(window_width / 2 - options.Width, window_height / 4 - options.Height, options.Width * 2, options.Height * 2);
            on_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 4*2 - options.Height, options.Width * 2, options.Height * 2); ;
            off_rectangle = new Rectangle(window_width / 2 - options.Width, window_height / 4*3 - options.Height, options.Width * 2, options.Height * 2);


            ///-------------------------------
            ///-------------------------------
            menuSong = Content.Load<SoundEffect>("menu");
            loop = Content.Load<SoundEffect>("Loop");
            intro = Content.Load<SoundEffect>("Intro");
            death = Content.Load<SoundEffect>("death");
            menuInstance = menuSong.CreateInstance();
            menuInstance.IsLooped = true;
            loopInstance = loop.CreateInstance();
            loopInstance.IsLooped = true;
            introInstance = intro.CreateInstance();
            deathInstance = death.CreateInstance();
           



        }

        private int where_is_min(int i, int j, int key)// 1 - снизу, 2 - сверху, 3 - слева, 4 - справа
        {
            int result = 0;
            if (field[(i + 21) % 20, j] == key - 1)
            {
                result = 4;
            }
            if (field[(i + 19) % 20, j] == key - 1)
            {
                result = 3;
            }
            if (field[i, (j + 19) % 20] == key - 1)
            {
                result = 2;
            }
            if (field[i, (j + 21) % 20] == key - 1)
            {
                result = 1;
            }
            return result;
        }
        private int where_is_max(int i, int j, int key)
        {
            int result = 0;
            if (field[(i + 21) % 20, j] == key + 1)
            {
                result = 4;
            }
            if (field[(i + 19) % 20, j] == key + 1)
            {
                result = 3;
            }
            if (field[i, (j + 19) % 20] == key + 1)
            {
                result = 2;
            }
            if (field[i, (j + 21) % 20] == key + 1)
            {
                result = 1;
            }
            return result;
        }
        private void draw_game()
        {

            if (_gameState == GameStates.Dead)
            {
                spriteBatch.DrawString(spritefont, "you are dead", new Vector2(window_width / 3, window_height / 4), Color.White, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spritefont, "your score is "+ score, new Vector2(window_width / 3, window_height / 3), Color.White, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0f);
            }
            if  (_gameState == GameStates.Playing)
            {
spriteBatch.DrawString(spritefont, "Score: "+score, new Vector2(0,0), Color.White);
            }
       
            
            for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        spriteBatch.Draw(big_field, new Vector2((i * 18 + 9) * scale, (j * 18 + 9)* scale), new Rectangle(0, 0, big_field.Width, big_field.Height),
                                     Color.White, 0, new Vector2(big_field.Width / 2, big_field.Height / 2), scale, SpriteEffects.None, 1f);
                        if (field[i, j] != 0)
                        {
                            if (field[i, j] == 1)//голова
                            {
                                switch (where_is_max(i, j, 1))
                                {
                                    case 4:
                                        rotation = -MathHelper.PiOver2;
                                        break;
                                    case 3:
                                        rotation = MathHelper.PiOver2;
                                        break;
                                    case 2:
                                        rotation = MathHelper.Pi;
                                        break;
                                    case 1:
                                        rotation = 0;
                                        break;
                                }
                                if (nyah > 0)
                                {
                                    nyah--;
                                }
                                spriteBatch.Draw(_gameState == GameStates.Dead ? snake_wow : (nyah == 0 ? snake_head : snake_nyah), new Vector2((x_head * 18 + 9) * scale, (y_head * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, rotation, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }
                            if (where_is_max(i, j, field[i, j]) == 0)//хвост
                            {
                                int temp = where_is_min(i, j, field[i, j]);
                                float rotation = 0;
                                if (temp == 2)
                                {
                                    rotation = 0;
                                }
                                if (temp == 1)
                                {
                                    rotation = MathHelper.Pi;
                                }
                                if (temp == 4)
                                {
                                    rotation = MathHelper.PiOver2;
                                }
                                if (temp == 3)
                                {
                                    rotation = -MathHelper.PiOver2;
                                }
                                tail_oddity++;
                                tail_oddity %= 32;
                                spriteBatch.Draw(tail_oddity < 16 ? snake_tail1 : snake_tail2, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, rotation, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }
                            int back = where_is_max(i, j, field[i, j]);
                            int forward = where_is_min(i, j, field[i, j]);
                            if (back == 1 && forward == 2)
                            {
                                spriteBatch.Draw(field[i, j] % 2 == 0 ? snake_body_even : snake_body_odd, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, 0, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }
                            if (back == 2 && forward == 1)
                            {
                                spriteBatch.Draw(field[i, j] % 2 == 0 ? snake_body_even : snake_body_odd, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, MathHelper.Pi, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }
                            if (back == 3 && forward == 4)
                            {
                                spriteBatch.Draw(field[i, j] % 2 == 0 ? snake_body_even : snake_body_odd, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, MathHelper.PiOver2, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }
                            if (back == 4 && forward == 3)
                            {
                                spriteBatch.Draw(field[i, j] % 2 == 0 ? snake_body_even : snake_body_odd, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, -MathHelper.PiOver2, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }

                            //-------------------------------------------
                            if (back == 3 && forward == 2)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, 0, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None
                                , 0f);
                                continue;
                            }

                            if (back == 2 && forward == 3)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, -MathHelper.PiOver2, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.FlipHorizontally
                                , 0f);
                                continue;
                            }

                            //-------------------dwfcqefq34fc-----------------

                            if (back == 2 && forward == 4)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, MathHelper.PiOver2, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }

                            if (back == 4 && forward == 2)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, 0, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.FlipHorizontally
                                , 0f);
                                continue;
                            }

                            //------------------------

                            if (back == 1 && forward == 4)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, MathHelper.PiOver2, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.FlipHorizontally, 0f);
                                continue;
                            }
                            if (back == 4 && forward == 1)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, -MathHelper.PiOver2, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.FlipVertically, 0f);
                                continue;
                            }///////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!WOWOWOWOWOW

                            if (back == 1 && forward == 3)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i * 18 + 9) * scale, (j * 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, -MathHelper.PiOver2, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.None, 0f);
                                continue;
                            }
                            if (back == 3 && forward == 1)
                            {
                                spriteBatch.Draw(snake_turn, new Vector2((i* 18 + 9) * scale, (j* 18 + 9) * scale), new Rectangle(18 * (skin - 1), 0, 18, 18),
                                    Color.White, 0, new Vector2(18 / 2, 18 / 2), scale, SpriteEffects.FlipVertically, 0f);
                                continue;
                            }

                        }
                    }
                }
            
            if (_gameState == GameStates.Menu)
            {
                draw_menu();
            }
        }
        private void draw_food()
        {
            spriteBatch.Draw(food, new Vector2((x_food * 18 + 9) * scale, (y_food * 18 + 9) * scale), new Rectangle(0, 0, food.Width, food.Height),
                        Color.White, 0, new Vector2(food.Width / 2, food.Height / 2), scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(squirrel, new Vector2((x_squirrel * 18 + 9) * scale, (y_squirrel * 18 + 9) * scale), new Rectangle(0, 0, squirrel.Width, squirrel.Height),
                       Color.White, 0, new Vector2(squirrel.Width / 2, squirrel.Height / 2), scale, SpriteEffects.None, 0f);
        }
        private void draw_menu()//
        {
            if (play_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y))){
                spriteBatch.Draw(play_pressed, new Vector2(window_width / 2, window_height / 3), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(play, new Vector2(window_width / 2, window_height / 3), new Rectangle(0, 0, play.Width, play.Height),
                                               Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            if (options_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
            {
                spriteBatch.Draw(options_pressed, new Vector2(window_width / 2, window_height / 3*2), new Rectangle(0, 0, options.Width, options.Height),
                                                Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(options, new Vector2(window_width / 2, window_height / 3 * 2), new Rectangle(0, 0, options.Width, options.Height),
                                               Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            



            //dodelat'

        }
        private void draw_options()
        {
            if (options_back.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
                {
                spriteBatch.Draw(back_pressed, new Vector2(window_width / 2, window_height / 4 ), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(back, new Vector2(window_width / 2, window_height / 4), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }

            if (difficulty_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
            {
                spriteBatch.Draw(difficulty_pressed, new Vector2(window_width / 2, window_height / 4*2), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(difficulty, new Vector2(window_width / 2, window_height / 4*2), new Rectangle(0, 0, play.Width, play.Height),
                                               Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            if (sound_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
            {
                spriteBatch.Draw(sound_pressed, new Vector2(window_width / 2, window_height / 4 * 3), new Rectangle(0, 0, options.Width, options.Height),
                                                Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(sound, new Vector2(window_width / 2, window_height / 4 * 3), new Rectangle(0, 0, options.Width, options.Height),
                                               Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }



        }
        private void draw_difficulty()
        {
            if (difficulty_back.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
            {
                spriteBatch.Draw(back_pressed, new Vector2(window_width / 2, window_height/5), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(back, new Vector2(window_width / 2, window_height / 5), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }

            if ((easy_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))|| _dif == difficulties.easy)
            {
                spriteBatch.Draw(easy_pressed, new Vector2(window_width / 2, window_height / 5*2 ), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(easy, new Vector2(window_width / 2, window_height / 5*2 ), new Rectangle(0, 0, play.Width, play.Height),
                                               Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            if ((normal_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))|| _dif == difficulties.normal)
            {
                spriteBatch.Draw(normal_pressed, new Vector2(window_width / 2, window_height / 5 * 3), new Rectangle(0, 0, options.Width, options.Height),
                                                Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(normal, new Vector2(window_width / 2, window_height / 5 * 3), new Rectangle(0, 0, options.Width, options.Height),
                                               Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            if ((hard_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))||_dif == difficulties.hard)
            {
                spriteBatch.Draw(hard_pressed, new Vector2(window_width / 2, window_height/5*4), new Rectangle(0, 0, options.Width, options.Height),
                                                Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(hard, new Vector2(window_width / 2, window_height/5*4), new Rectangle(0, 0, options.Width, options.Height),
                                               Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
        }
        private void draw_sound()
        {
            if (sound_back.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
            {
                spriteBatch.Draw(back_pressed, new Vector2(window_width / 2, window_height / 4), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(back, new Vector2(window_width / 2, window_height / 4), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }

            if (on_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y))||soundison == true)
            {
                spriteBatch.Draw(on_pressed, new Vector2(window_width / 2, window_height / 4 * 2), new Rectangle(0, 0, play.Width, play.Height),
                                                Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(on, new Vector2(window_width / 2, window_height / 4 * 2), new Rectangle(0, 0, play.Width, play.Height),
                                               Color.White, 0, new Vector2(play.Width / 2, play.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            if (off_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y))||soundison == false)
            {
                spriteBatch.Draw(off_pressed, new Vector2(window_width / 2, window_height / 4 * 3), new Rectangle(0, 0, options.Width, options.Height),
                                                Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.Draw(off, new Vector2(window_width / 2, window_height / 4 * 3), new Rectangle(0, 0, options.Width, options.Height),
                                               Color.White, 0, new Vector2(options.Width / 2, options.Height / 2), 2f, SpriteEffects.None, 1f);
            }

        }
        private void move()
        {
            
            move_direction = temp_move_direction;

            if (move_direction == 1)
            {
                y_head = (y_head + 19) % 20;
            }
            if (move_direction == 2)
            {
                y_head = (y_head + 21) % 20;
            }
            if (move_direction == 3)
            {
                x_head = (x_head + 21) % 20;
            }
            if (move_direction == 4)
            {
                x_head = (x_head + 19) % 20;
            }

            if (field[x_head, y_head] > 1)
            {
                _gameState = GameStates.Dead;
                loopInstance.Stop();
                introInstance.Stop();
                intromusic = false;

            }

            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    if (field[i, j] != 0)
                    {
                        field[i, j]++;
                        if (field[i, j] > length)
                            field[i, j] = 0;
                    }

            field[x_head, y_head] = 1;

            // ---------------------------------------




            if (x_head == x_food && y_head == y_food)
            {
                score++;
                squirrel_count++;
                while (field[x_food, y_food] !=0)
                {

                    x_food = rand.Next(20);
                    y_food = rand.Next(20);
                }
                length++;
                nyah = 60;
            }
            if (squirrel_count == 5)
            {
                squirrel_count = 0;
                squirrel_countdown = 300;
                do
                {
                    
                    x_squirrel = rand.Next(20);
                    y_squirrel = rand.Next(20);

                }
                while (field[x_squirrel, y_squirrel]!= 0 && x_squirrel == x_food && x_squirrel == y_food);
                
            }
            if (x_head == x_squirrel && y_head == y_squirrel)
            {
                score += 4;
                x_squirrel = -13;
                y_squirrel = -13;
                length += 3;
            }
            if (squirrel_countdown == 0)
            {
                x_squirrel = -13;
                y_squirrel = -13;
            }



        }
        private void keyboard_move()//хелп
        {

            currentstate = Keyboard.GetState();
            curmouse = Mouse.GetState();
            if (currentstate.IsKeyDown(Keys.W) && move_direction != 2)
            {
                temp_move_direction = 1;
                // rotation = 0;
            }
            if (currentstate.IsKeyDown(Keys.S) && move_direction != 1)
            {
                temp_move_direction = 2;
                //rotation = MathHelper.Pi;
            }
            if (currentstate.IsKeyDown(Keys.A) && move_direction != 3)
            {
                temp_move_direction = 4;
                //rotation = -MathHelper.PiOver2;

            }
            if (currentstate.IsKeyDown(Keys.D) && move_direction != 4)
            {
                temp_move_direction = 3;
                //rotation = MathHelper.PiOver2;
            }
            if (currentstate.IsKeyDown(Keys.Space) && oldstate.IsKeyUp(Keys.Space))
            {
                _gameState = GameStates.Dead;
                oldstate = currentstate;
            }
           

            if ((currentstate.IsKeyDown(Keys.RightShift) || currentstate.IsKeyDown(Keys.LeftShift)))
                { 
            
                divide_speed = 20;
                
            } 

            else 
            {
                divide_speed = 10;
            }
            oldstate = currentstate;
           

        }
        private void pause()//хелп
        {
            currentstate = Keyboard.GetState();
            if (currentstate.IsKeyDown(Keys.Space) && oldstate.IsKeyUp(Keys.Space))
            {
               
                _gameState = GameStates.Playing;
                
               
            }
           
        }
        private void tomenu()//
        {
            
            if (currentstate.IsKeyDown(Keys.Escape))
            {
                _gameState = GameStates.Menu;
                loopInstance.Stop();
                introInstance.Stop();
                menumusic = false;
              
               deathmusic = false;
                intromusic = false;
                score = 0;
            }

        }
        private void menu()//
        {
            if  (play_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
                {
                
                menuInstance.Stop();
                
                _gameState = GameStates.Playing;
                Initialize();

            }

            if (options_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _gameState = GameStates.Menu_options;

            }
        }
        private void optionsmethod()
        {
            if (options_back.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _gameState = GameStates.Menu;

            }
            if (difficulty_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _gameState = GameStates.Menu_difficulty;

            }
            if (sound_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _gameState = GameStates.Menu_sound;

            }


        }
        private void difficulty_method()
        {
            if (difficulty_back.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _gameState = GameStates.Menu_options;

            }
            if (easy_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _dif = difficulties.easy;


            }
            if (normal_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _dif = difficulties.normal;

            }
            if (hard_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _dif = difficulties.hard;

            }
        }
        private void sound_method()
        {
            if (sound_back.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                _gameState = GameStates.Menu_options;

            }
            if (on_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                soundison = true;
                soundturning(1);


            }
            if (off_rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                soundison = false;
                soundturning(0);

            }
        }
        private void soundturning(int volume)
        {
            menuInstance.Volume = volume;
            introInstance.Volume = volume;
            loopInstance.Volume = volume;
            deathInstance.Volume = volume;

        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }       
        ///--------------------------------------------------------------------------
        ///--------------------------------------------------------------------------
        ///--------------------------------------------------------------------------       
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calli1111ng base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           
            menumusic = false;
            graphics.PreferredBackBufferWidth = window_width;
            graphics.PreferredBackBufferHeight = window_height;
           //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
            // TODO: Add your initialization logic here
            init_logic_field();
            base.Initialize();
        }

        /// <summary>
        /// Загрузка графического контета игры. Метод будет вызван один раз.
        /// </summary>
        protected override void LoadContent()
        {
            // Создание экземпляра класс SpriteBatсh.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            init_field();
            init_snake();
            init_food();
            init_menu();
            loop = Content.Load<SoundEffect>("loop");
            intro = Content.Load<SoundEffect>("intro");

        }
       

        /// <summary>
        /// Выгружаем контент, который создали во время игры, без использование ContentManager
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Реализация игровой логики должны быть в данном методе.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)//хелп
        {
            if (soundison == false)
            {
                soundturning(0);
            }
            keyboard_move();
            if (_gameState == GameStates.Playing)
            {

                switch (_dif)
                {
                    case difficulties.easy:
                        divide_speed = 13;
                        break;
                    case difficulties.normal:
                        divide_speed = 10;
                        break;
                    case difficulties.hard:
                        divide_speed = 5;
                        break;



                }
                menuInstance.Stop();
                menumusic = false;

                if (intromusic == false)
                {
                    intromusic = true;
                    introInstance.Play();

                }
                if (!(introInstance.State == SoundState.Playing) && intromusic == true)
                {
                    loopInstance.Play();

                }


                delta = (delta + 1) % divide_speed;
                if (delta == 0)
                {
                    move();
                    if(skinrand)
                    skin = (skin++) % 16 + 1;
                }
                tomenu();

            }

            else if (_gameState == GameStates.Dead)
            {

                if (deathmusic == false)
                {
                    deathmusic = true;
                    deathInstance.Play();
                }
                pause();
                tomenu();
            }
            
        
            else if (_gameState == GameStates.Menu)
            {
                menu();
                if (menumusic == false)
                {
                    menumusic = true;
                    

                    menuInstance.Play();
                   
                }
            }
            else if (_gameState == GameStates.Menu_options)
            {
                optionsmethod();
            }
            else if (_gameState == GameStates.Menu_difficulty)
            {
                difficulty_method();
            }
            else if (_gameState == GameStates.Menu_sound)
            {
                sound_method();
            }

            oldmouse = curmouse;
            squirrel_countdown--;

        }

        /// <summary>
        /// Метод для рендера
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //ВАЖНО, для того, чтобы работала сортировка спрайтов надо в spriteBatch.Begin() передать SpriteSortMode.BackToFront или SpriteSortMode.FrontToBack

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp);
            if (_gameState == GameStates.Playing|| _gameState == GameStates.Dead)
            {
                draw_game();
                draw_food();
            }
            else if (_gameState == GameStates.Menu)
            {
                draw_menu();
            }
            else if (_gameState == GameStates.Menu_options)
            {
                draw_options();
            }
            else if (_gameState == GameStates.Menu_difficulty)
            {
                draw_difficulty();
            }
            else if (_gameState == GameStates.Menu_sound)
            {
                draw_sound();
            }
                

                spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}