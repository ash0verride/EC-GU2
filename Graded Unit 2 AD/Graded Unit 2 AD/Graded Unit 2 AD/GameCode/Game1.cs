using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;

namespace GradedUnit2AD
{
    /// <summary>
    /// Used to manage the main menu and pause menu
    /// </summary>
    #region States
    // The current screen the game is in
    enum GameState
    {
        playing,
        paused,
        mainmenu
    }
    // The highlighted selection
    enum SelectState
    {
        newGame,
        loadGame,
        exitGame,
        resumeGame,
        saveGame,
        menuGame
    }
    #endregion
    /// <summary>
    /// This is the main type for the game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Creates the games variables
        #region Variables

        GameState gameState;
        SelectState selectState;
        LevelReset resetState;

        bool menuPrompt;
        float screenSwipe;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RenderTarget2D preCanvas;
        RenderTarget2D lightMask;
        RenderTarget2D finalCanvas;
        Effect lightShader;

        Song score01; 
        Song score02;
        Song score03;
        Song score04;

        SoundEffect sfxMenuConfirm;
        SoundEffect sfxMenuDecline;
        SoundEffect sfxMenuMovement;
        SoundEffect sfxMenuPause;
        public static SoundEffect sfxNpcBear;
        public static SoundEffect sfxNpcDog;
        public static SoundEffect sfxNpcEnemy;
        public static SoundEffect sfxNpcFollower;
        public static SoundEffect sfxNpcGuard;
        SoundEffect sfxPlayerCaught;
        SoundEffect sfxPlayerDoor;
        SoundEffect sfxPlayerInteract;
        SoundEffect sfxPlayerItem;
        public static SoundEffect sfxPlayerLamp;

        KeyboardState currKb;
        KeyboardState oldKb;

        public static readonly Random RNG = new Random();

        int level;
        Level[] levelStats;
        LargeBackdrops[] levelBackground;
        StaticGraphic[] levelTerrain;
        StaticGraphic blankScreen;
        StaticGraphic mainMenuScreen;
        StaticGraphic pauseMenuScreen;
        AnimatedGraphic[] MenuStars;
        RevealGraphic innerBar;
        StaticGraphic outterBar;
        float lampAlpha;

        Camera playerCam;
        PlayerActor player;
        LightGraphic lamp;

        List<NpcActor> npc;
        List<InventoryItems> items;
        List<SmokeEmitter> smokeEmitter;
        List<ItemPickup> chests;
        List<DoorTrigger> door;
        public static Texture2D smokeTxr;
        public static SpriteFont npcFont;

        SpriteFont titleFont;
        SpriteFont menuFont;
        SpriteFont hintFont;

        GameSave gameSave;
        string saveFile;
        bool gameSaveError;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Sets screen variables
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;

            // Window Title
            Window.Title = "Left Alone";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Sets the game to the main menu
            gameState = GameState.mainmenu;
            selectState = SelectState.newGame;
            menuPrompt = false;
            screenSwipe = 0f;
            MenuStars = new AnimatedGraphic[8];

            // Allows the music to repeat
            MediaPlayer.IsRepeating = true;

            // Used for creating the light mask
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            preCanvas = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            lightMask = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            finalCanvas = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

            // Sets the level and prevents repetiton
            level = 0;
            resetState = LevelReset.playing;

            // Sets the games collision maps and level variables
            levelStats = new Level[11];
            for (int i = 0; i < levelStats.Length; i++)
                levelStats[i] = new Level(i);

            // Creates the level backgrounds
            levelBackground = new LargeBackdrops[11];
            levelTerrain = new StaticGraphic[11];

            // Creates the level content
            npc = new List<NpcActor>();
            items = new List<InventoryItems>();
            chests = new List<ItemPickup>();
            smokeEmitter = new List<SmokeEmitter>();
            door = new List<DoorTrigger>();

            // Sets the save files root
            saveFile = "gameSave.dat";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of the content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads different game effects content
            smokeTxr = Content.Load<Texture2D>("Sprites//ItemSprites//PotatoSmoke");
            npcFont = Content.Load<SpriteFont>("Fonts//npcFont");
            lightShader = Content.Load<Effect>("Effects//LightShader");

            // Loads the games fonts
            titleFont = Content.Load<SpriteFont>("Fonts//TitleFont");
            menuFont = Content.Load<SpriteFont>("Fonts//menuFont");
            hintFont = Content.Load<SpriteFont>("Fonts//HintFont");

            // Loads the Games Music
            score01 = Content.Load<Song>("Score//Adrift_in_the_Cosmos");
            score02 = Content.Load<Song>("Score//Quiet_Breeches_in_Space");
            score03 = Content.Load<Song>("Score//The_Forest_Beckons_You_sans_Wind");
            score04 = Content.Load<Song>("Score//Beyond_the_Horizon");

            // Loads the Games Sound Effects
            sfxMenuConfirm = Content.Load<SoundEffect>("SFXs//MenuConfirm");
            sfxMenuDecline = Content.Load<SoundEffect>("SFXs//MenuDecline");
            sfxMenuMovement = Content.Load<SoundEffect>("SFXs//MenuMovement");
            sfxMenuPause = Content.Load<SoundEffect>("SFXs//MenuPause");
            sfxNpcBear = Content.Load<SoundEffect>("SFXs//NpcBear");
            sfxNpcDog = Content.Load<SoundEffect>("SFXs//NpcDog");
            sfxNpcEnemy = Content.Load<SoundEffect>("SFXs//NpcEnemy");
            sfxNpcFollower = Content.Load<SoundEffect>("SFXs//NpcFollower");
            sfxNpcGuard = Content.Load<SoundEffect>("SFXs//NpcGuard");
            sfxPlayerCaught = Content.Load<SoundEffect>("SFXs//PlayerCaught");
            sfxPlayerDoor = Content.Load<SoundEffect>("SFXs//PlayerDoor");
            sfxPlayerInteract = Content.Load<SoundEffect>("SFXs//PlayerInteract");
            sfxPlayerItem = Content.Load<SoundEffect>("SFXs//PlayerItem");
            sfxPlayerLamp = Content.Load<SoundEffect>("SFXs//PlayerLamp");

            // Loads the player and their differet outfits (CONTENT UNFINISHED SO NOT IMPLEMENTED)
            player = new PlayerActor(new Rectangle(200, 250, 50, 100), 
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft01"), 
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"),
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft01"),
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"),
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft01"),
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"),
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft01"),
                Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"),
                12, 9, 1, Vector2.Zero, 2f);

            // Loads the lamp and its aura
            lamp = new LightGraphic(new Rectangle(0, 0, 256, 256), Content.Load<Texture2D>("Sprites//ItemSprites//lampImage"), Content.Load<Texture2D>("Sprites//ItemSprites//lightaura"));
            
            // Loads the NPC's into memory
            npc.Add(new NpcActor(new Rectangle(200, 100, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront02"), 12, 9, 1, Vector2.Zero, 1.5f, AiState.stop, AiType.friendly, null, null));
            // CONTENT UNFINISHED SO OTHER SPRITESHEETS NOT IMPLEMENTED

            // Loads the games item pickups into memory
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//AmmoHere"), Content.Load<Texture2D>("Sprites//ItemSprites//AmmoGone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//HatHere"), Content.Load<Texture2D>("Sprites//ItemSprites//HatGone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//JacketHere"), Content.Load<Texture2D>("Sprites//ItemSprites//JacketGone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//LampHere"), Content.Load<Texture2D>("Sprites//ItemSprites//LampGone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//Map01Here"), Content.Load<Texture2D>("Sprites//ItemSprites//Map01Gone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//Map02Here"), Content.Load<Texture2D>("Sprites//ItemSprites//Map02Gone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//PickaxeHere"), Content.Load<Texture2D>("Sprites//ItemSprites//PickaxeGone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//RadioHere"), Content.Load<Texture2D>("Sprites//ItemSprites//RadioGone"), new InventoryItems("test")));
            chests.Add(new ItemPickup(new Rectangle(0, 0, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//SamHatHere"), Content.Load<Texture2D>("Sprites//ItemSprites//SamHatGone"), new InventoryItems("test")));

            // Loads Fade screen and menu screens
            blankScreen = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Interface//BlankScreen"));
            mainMenuScreen = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Interface//MainMenuScreen"));
            pauseMenuScreen = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Interface//PauseMenuScreen"));

            // Makes stars for the main menu
            for (int i = 0; i < MenuStars.Length; i++)
            {
                MenuStars[i] = new AnimatedGraphic(new Rectangle(RNG.Next(0, graphics.PreferredBackBufferWidth / 3), RNG.Next(0, graphics.PreferredBackBufferHeight / 3), RNG.Next(5, 20), RNG.Next(6, 24)), Content.Load<Texture2D>("Sprites//Interface//StarSS"), RNG.Next(1, 6), 7, 1);
            }

            // Loads the lamps UI bar
            innerBar = new RevealGraphic(new Rectangle(0, 0, 370, 148), Content.Load<Texture2D>("Sprites//Interface//InnerBar"), 5);
            outterBar = new StaticGraphic(new Rectangle(0, 0, 393, 148), Content.Load<Texture2D>("Sprites//Interface//OutterBar"));

            // Loads and sets the games backgrounds to the levels
            levelBackground[0] = new LargeBackdrops(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//BackgroundDiary01"), true, true);
            levelBackground[1] = new LargeBackdrops(new Rectangle(0, 0, 800, 900), Content.Load<Texture2D>("Sprites//Levels//House//BackgroundHouse01"), true, false);
            levelBackground[2] = new LargeBackdrops(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//BackgroundDiary01"), true, true);
            levelBackground[3] = new LargeBackdrops(new Rectangle(0, 0, 800, 900), Content.Load<Texture2D>("Sprites//Levels//City//BackgroundCity01"), true, false);
            levelBackground[4] = new LargeBackdrops(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//BackgroundDiary01"), true, true);
            levelBackground[5] = new LargeBackdrops(new Rectangle(0, 0, 800, 1200), Content.Load<Texture2D>("Sprites//Levels//Forest//BackgroundForest01"), true, false);
            levelBackground[6] = new LargeBackdrops(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//BackgroundDiary01"), true, true);
            levelBackground[7] = new LargeBackdrops(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Cave//BackgroundCave01"), true, true);
            levelBackground[8] = new LargeBackdrops(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//BackgroundDiary01"), true, true);
            levelBackground[9] = new LargeBackdrops(new Rectangle(0, 0, 800, 1000), Content.Load<Texture2D>("Sprites//Levels//City//BackgroundCity01"), true, false);
            levelBackground[10] = new LargeBackdrops(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//BackgroundDiary01"), true, true);

            // Loads and sets the games maps to the levels
            levelTerrain[0] = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//MapDiary00"));
            levelTerrain[1] = new StaticGraphic(new Rectangle(0, 0, 2400, 650), Content.Load<Texture2D>("Sprites//Levels//House//MapHouse01"));
            levelTerrain[2] = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//MapDiary02"));
            levelTerrain[3] = new StaticGraphic(new Rectangle(0, 0, 1600, 900), Content.Load<Texture2D>("Sprites//Levels//City//MapCity01"));
            levelTerrain[4] = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//MapDiary04"));
            levelTerrain[5] = new StaticGraphic(new Rectangle(0, 0, 4000, 1200), Content.Load<Texture2D>("Sprites//Levels//Forest//MapForest01"));
            levelTerrain[6] = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//MapDiary06"));
            levelTerrain[7] = new StaticGraphic(new Rectangle(0, 0, 1600, 600), Content.Load<Texture2D>("Sprites//Levels//Cave//MapCave01"));
            levelTerrain[8] = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//MapDiary08"));
            levelTerrain[9] = new StaticGraphic(new Rectangle(0, 0, 2400, 1000), Content.Load<Texture2D>("Sprites//Levels//Camp//MapCamp01"));
            levelTerrain[10] = new StaticGraphic(new Rectangle(0, 0, 800, 600), Content.Load<Texture2D>("Sprites//Levels//Diary//MapDiary10"));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Updates the current keyboard
            currKb = Keyboard.GetState();

            // Updates the game when playing
            if (gameState == GameState.playing)
            {
                // Updates and animates the player
                player.updateMe(currKb, levelStats[level].Map, items);
                player.animateMe(gameTime);

                // Updates the lamp
                lamp.updateMe(player, gameTime);

                for (int i = 0; i < npc.Count; i++)
                {
                    // Lets the player interact with the NPC'c
                    if (player.Rect.Intersects(npc[i].Rect) && ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O))))
                    {
                        sfxPlayerInteract.CreateInstance().Play();
                        npc[i].Interact = true;
                    }

                    // Resets the level if attacked by a bear
                    if (player.Rect.Intersects(npc[i].Rect) && npc[i].Type == AiType.bear)
                    {
                        sfxPlayerCaught.CreateInstance().Play();
                        MediaPlayer.Stop();
                        MediaPlayer.Play(score03);
                        level5();
                    }

                    // Resets the level if attacked by a invader
                    if (player.Rect.Intersects(npc[i].Rect) && npc[i].Type == AiType.enemy)
                    {
                        sfxPlayerCaught.CreateInstance().Play();
                        MediaPlayer.Stop();
                        MediaPlayer.Play(score03);
                        level7();
                    }

                    // Updates the NPC's and animates them
                    npc[i].updateMe(levelStats[level].Map, player, gameTime, items);
                    npc[i].animateMe(gameTime);
                }

                // Updates the smoke emitters
                for (int i = 0; i < smokeEmitter.Count; i++)
                {
                    smokeEmitter[i].updateMe(gameTime);
                }

                for (int i = 0; i < chests.Count; i++)
                {
                    // Interacts with items if intersecting and selected
                    if (player.Rect.Intersects(chests[i].Rect) && ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O))))
                    {
                        sfxPlayerItem.CreateInstance().Play();
                        chests[i].Interact = true;
                    }

                    // Updates item pickups
                    chests[i].updateMe(gameTime, items, player);
                }

                for (int i = 0; i < door.Count; i++)
                {
                    // Checks to see if player had interacted with doors
                    if (player.Rect.Intersects(door[i].Rect) && ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O))))
                    {
                        sfxPlayerDoor.CreateInstance().Play();
                        door[i].Interact = true;
                        resetState = door[i].Reset;
                    }

                    // Updates the doors
                    door[i].updateMe(gameTime, items, player);
                }

                // Updates the player camera
                playerCam.updateMe(new Vector2(player.Rect.X, player.Rect.Y), player.Velo, graphics, player.Rect, levelStats[level]);

                // Updates the player backgrounds
                levelBackground[level].updateMe(playerCam.Pos);

                // Show Lamp UI
                if (lamp.Flicker < 5)
                {
                    if (lampAlpha < 1)
                        lampAlpha += 0.01f;
                }
                // Hide Lamp UI
                else if (lampAlpha > 0)
                {
                    lampAlpha -= 0.01f;
                }

                // Hide Screen transition
                if (screenSwipe > 0)
                {
                    screenSwipe -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                // Goes to Pause Menu
                if (currKb.IsKeyDown(Keys.Escape) && oldKb.IsKeyUp(Keys.Escape))
                {
                    sfxMenuPause.CreateInstance().Play();
                    MediaPlayer.Volume = 0.1f;
                    selectState = SelectState.resumeGame;
                    gameState = GameState.paused;
                }
            }
            // Updates Pause Menu
            else if (gameState == GameState.paused)
            {
                // Return to the game
                if (currKb.IsKeyDown(Keys.Escape) && oldKb.IsKeyUp(Keys.Escape))
                {
                    sfxMenuPause.CreateInstance().Play();
                    MediaPlayer.Volume = 0.5f;
                    menuPrompt = false;
                    gameState = GameState.playing;
                }

                if (selectState == SelectState.resumeGame)
                {
                    // Return to the game
                    if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                    {
                        sfxMenuConfirm.CreateInstance().Play();
                        MediaPlayer.Volume = 0.5f;
                        gameState = GameState.playing;
                    }
                    else if ((currKb.IsKeyDown(Keys.S) && oldKb.IsKeyUp(Keys.S)) || (currKb.IsKeyDown(Keys.Down) && oldKb.IsKeyUp(Keys.Down)))
                    {
                        sfxMenuMovement.CreateInstance().Play();
                        selectState = SelectState.saveGame;
                    }
                }
                else if (selectState == SelectState.saveGame)
                {
                    // Save the game
                    if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                    {
                        sfxMenuConfirm.CreateInstance().Play();
#if WINDOWS
                        // Populates empty array with preset values
                        gameSave = new GameSave(level);

                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i].Item == "Lamp")
                                gameSave.Lamp = true;
                            else if (items[i].Item == "Warm Jacket")
                                gameSave.Jacket = true;
                            else if (items[i].Item == "Top Hat")
                                gameSave.Hat = true;
                        }

                        // Creates a new save file with the populated values
                        FileStream stream = File.Open(saveFile, FileMode.OpenOrCreate);
                        try
                        {
                            XmlSerializer serialiser;
                            serialiser = new XmlSerializer(typeof(GameSave));
                            serialiser.Serialize(stream, gameSave);
                        }
                        finally
                        {
                            stream.Close();
                        }
#endif
                    }
                    else if ((currKb.IsKeyDown(Keys.W) && oldKb.IsKeyUp(Keys.W)) || (currKb.IsKeyDown(Keys.Up) && oldKb.IsKeyUp(Keys.Up)))
                    {
                        sfxMenuMovement.CreateInstance().Play();
                        selectState = SelectState.resumeGame;
                    }
                    else if ((currKb.IsKeyDown(Keys.S) && oldKb.IsKeyUp(Keys.S)) || (currKb.IsKeyDown(Keys.Down) && oldKb.IsKeyUp(Keys.Down)))
                    {
                        sfxMenuMovement.CreateInstance().Play();
                        selectState = SelectState.menuGame;
                    }
                }
                else
                {
                    // Show confirm message
                    if (menuPrompt)
                    {
                        // return to main menu
                        if ((currKb.IsKeyDown(Keys.Q) && oldKb.IsKeyUp(Keys.Q)) || (currKb.IsKeyDown(Keys.I) && oldKb.IsKeyUp(Keys.I)))
                        {
                            sfxMenuConfirm.CreateInstance().Play();
                            MediaPlayer.Play(score01);
                            MediaPlayer.Volume = 0.5f;
                            gameState = GameState.mainmenu;
                            selectState = SelectState.newGame;
                            menuPrompt = false;
                        }
                        // Cancel prompt
                        else if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                        {
                            sfxMenuDecline.CreateInstance().Play();
                            menuPrompt = false;
                        }
                    }
                    else
                    {
                        // Attempt to return to menu
                        if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                        {
                            sfxMenuConfirm.CreateInstance().Play();
                            menuPrompt = true;
                        }
                        else if ((currKb.IsKeyDown(Keys.W) && oldKb.IsKeyUp(Keys.W)) || (currKb.IsKeyDown(Keys.Up) && oldKb.IsKeyUp(Keys.Up)))
                        {
                            sfxMenuMovement.CreateInstance().Play();
                            selectState = SelectState.saveGame;
                        }
                    }
                }
            }
            // Update Main Menu
            else
            {
                // Start playing music at half volume
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(score01);
                    MediaPlayer.Volume = 0.5f;
                }

                // Animate Stars
                for (int i = 0; i < MenuStars.Length; i++)
                    MenuStars[i].animateMe(gameTime);

                    if (selectState == SelectState.newGame)
                    {
                        // Start new Game
                        if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                        {
                            sfxMenuConfirm.CreateInstance().Play();
                            resetState = LevelReset.level0;
                            MediaPlayer.Stop();
                            MediaPlayer.Play(score02);
                            gameState = GameState.playing;
                        }
                        else if ((currKb.IsKeyDown(Keys.S) && oldKb.IsKeyUp(Keys.S)) || (currKb.IsKeyDown(Keys.Down) && oldKb.IsKeyUp(Keys.Down)))
                        {
                            sfxMenuMovement.CreateInstance().Play();
                            selectState = SelectState.loadGame;
                        }
                    }
                    else if (selectState == SelectState.loadGame)
                    {
                        if (gameSaveError == false)
                        {
                            // Load Game
                            if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                            {
#if WINDOWS
                                // Checks if a file exists already
                                if (!File.Exists(saveFile))
                                {
                                    gameSaveError = true;
                                }
                                else
                                {
                                    // Loads a previously made save file
                                    FileStream stream = File.Open(saveFile, FileMode.OpenOrCreate, FileAccess.Read);
                                    try
                                    {
                                        XmlSerializer serialiser;
                                        serialiser = new XmlSerializer(typeof(GameSave));
                                        gameSave = (GameSave)serialiser.Deserialize(stream);
                                    }
                                    // Load failed display message
                                    catch
                                    {
                                        sfxMenuDecline.CreateInstance().Play();
                                        gameSaveError = true;
                                    }
                                    finally
                                    {
                                        stream.Close();
                                    }

                                    // Load GameSave to Game
                                    if (gameSaveError == false)
                                    {
                                        sfxMenuConfirm.CreateInstance().Play();

                                        level = gameSave.Level;

                                        if (gameSave.Lamp)
                                            items.Add(new InventoryItems("Lamp"));
                                        if (gameSave.Jacket)
                                            items.Add(new InventoryItems("Jacket"));
                                        if (gameSave.Hat)
                                            items.Add(new InventoryItems("Hat"));

                                        resetState = LevelReset.level0;
                                        resetState += gameSave.Level;

                                        MediaPlayer.Stop();
                                        if (gameSave.Level < 5)
                                        {
                                            MediaPlayer.Play(score02);
                                        }
                                        else if (gameSave.Level > 8)
                                        {
                                            MediaPlayer.Play(score04);
                                        }
                                        else
                                            MediaPlayer.Play(score03);

                                        gameState = GameState.playing;
                                    }
                                }
#endif
                            }
                            else if ((currKb.IsKeyDown(Keys.W) && oldKb.IsKeyUp(Keys.W)) || (currKb.IsKeyDown(Keys.Up) && oldKb.IsKeyUp(Keys.Up)))
                            {
                                sfxMenuMovement.CreateInstance().Play();
                                selectState = SelectState.newGame;
                            }
                            else if ((currKb.IsKeyDown(Keys.S) && oldKb.IsKeyUp(Keys.S)) || (currKb.IsKeyDown(Keys.Down) && oldKb.IsKeyUp(Keys.Down)))
                            {
                                sfxMenuMovement.CreateInstance().Play();
                                selectState = SelectState.exitGame;
                            }
                        }
                        else
                        {
                            // Close game load error
                            if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                            {
                                sfxMenuConfirm.CreateInstance().Play();
                                gameSaveError = false;
                            }
                        }

                    }
                    else
                    {
                        // Exit Game
                        if ((currKb.IsKeyDown(Keys.Space) && oldKb.IsKeyUp(Keys.Space)) || (currKb.IsKeyDown(Keys.E) && oldKb.IsKeyUp(Keys.E)) || (currKb.IsKeyDown(Keys.O) && oldKb.IsKeyUp(Keys.O)))
                        {
                            MediaPlayer.Stop();
                            this.Exit();
                        }
                        else if ((currKb.IsKeyDown(Keys.W) && oldKb.IsKeyUp(Keys.W)) || (currKb.IsKeyDown(Keys.Up) && oldKb.IsKeyUp(Keys.Up)))
                        {
                            sfxMenuMovement.CreateInstance().Play();
                            selectState = SelectState.loadGame;
                        }
                    }
            }

            // Reset the game using the method when it is equal to reset state
            if (resetState != LevelReset.playing)
            {
                if (resetState == LevelReset.level0)
                    level0();
                else if (resetState == LevelReset.level1)
                    level1();
                else if (resetState == LevelReset.level2)
                    level2();
                else if (resetState == LevelReset.level3)
                    level3();
                else if (resetState == LevelReset.level4)
                    level4();
                else if (resetState == LevelReset.level5)
                    level5();
                else if (resetState == LevelReset.level6)
                    level6();
                else if (resetState == LevelReset.level7)
                    level7();
                else if (resetState == LevelReset.level8)
                    level8();
                else if (resetState == LevelReset.level9)
                    level9();
                else if (resetState == LevelReset.level10)
                    level10();
                else if (resetState == LevelReset.end)
                    endGame();
            }

            // Updates old KeyBoard
            oldKb = currKb;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draws game when playing
            if (gameState == GameState.playing)
            {
                // Sets light mask canvas
                GraphicsDevice.SetRenderTarget(preCanvas);
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, playerCam.getCam());

                // Draws the level
                levelBackground[level].drawMe(spriteBatch);
                levelTerrain[level].drawMe(spriteBatch);

                // Draws the level items and NPC's
                for (int i = 0; i < smokeEmitter.Count; i++)
                    smokeEmitter[i].drawMe(spriteBatch);
                for (int i = 0; i < chests.Count; i++)
                    chests[i].drawMe(spriteBatch);
                for (int i = 0; i < door.Count; i++)
                    door[i].drawMe(spriteBatch);
                for (int i = 0; i < npc.Count; i++)
                    npc[i].drawMe(spriteBatch);

                // Draws the player and the lamp
                player.drawMe(spriteBatch);
                lamp.drawMe(spriteBatch);

                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Draws over the mask
                GraphicsDevice.SetRenderTarget(lightMask);
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, playerCam.getCam());
                levelBackground[level].drawMe(spriteBatch);
                levelTerrain[level].drawMe(spriteBatch);
                for (int i = 0; i < npc.Count; i++)
                    npc[i].drawMe(spriteBatch);
                player.drawMe(spriteBatch);
                lamp.drawMask(spriteBatch, player);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Lights up the canvas
                GraphicsDevice.SetRenderTarget(finalCanvas);
                lightShader.Parameters["maskTexture"].SetValue(lightMask);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, lightShader);
                spriteBatch.Draw(preCanvas, Vector2.Zero, Color.White);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Draws the game
                spriteBatch.Begin();
                spriteBatch.Draw(finalCanvas, Vector2.Zero, Color.White);
                innerBar.drawMe(spriteBatch, lamp.Flicker, lampAlpha);
                outterBar.hideMe(spriteBatch, lampAlpha);
                blankScreen.fadeMe(spriteBatch, screenSwipe);
                spriteBatch.End();
            }
            // Draws the pause menu
            else if (gameState == GameState.paused)
            {
                spriteBatch.Begin();
                pauseMenuScreen.drawMe(spriteBatch);
                spriteBatch.DrawString(titleFont, "Paused", new Vector2(280, 60), Color.White);
                spriteBatch.DrawString(menuFont, "Resume", new Vector2(340, 320), Color.White);
                spriteBatch.DrawString(menuFont, "Save Game", new Vector2(322, 380), Color.White);
                spriteBatch.DrawString(menuFont, "Main Menu", new Vector2(310, 440), Color.White);
                spriteBatch.DrawString(hintFont, "Press E to select", new Vector2(340, 490), Color.White);

                if (selectState == SelectState.resumeGame)
                {
                    spriteBatch.DrawString(menuFont, "Resume", new Vector2(340, 320), Color.Violet);
                }
                else if (selectState == SelectState.saveGame)
                {
                    spriteBatch.DrawString(menuFont, "Save Game", new Vector2(322, 380), Color.Violet);
                }
                else
                {
                    spriteBatch.DrawString(menuFont, "Main Menu", new Vector2(310, 440), Color.Violet);
                    if (menuPrompt)
                    {
                        spriteBatch.DrawString(hintFont, "   Are You Sure You Want to Exit\n Level Progress Will Not Be Saved\nPress E to Cancel Or Q to Continue", new Vector2(270, 210), Color.Violet);
                    }
                }
                spriteBatch.End();
            }
            // Draws the main menu
            else
            {
                spriteBatch.Begin();
                mainMenuScreen.drawMe(spriteBatch);
                for (int i = 0; i < MenuStars.Length; i++)
                    MenuStars[i].drawMe(spriteBatch);
                spriteBatch.DrawString(titleFont, "Left Alone", new Vector2(80, 80), Color.White);
                spriteBatch.DrawString(menuFont, "New Game", new Vector2(440, 320), Color.White);
                spriteBatch.DrawString(menuFont, "Continue", new Vector2(440, 385), Color.White);
                spriteBatch.DrawString(menuFont, "Exit Game", new Vector2(440, 450), Color.White);
                spriteBatch.DrawString(hintFont, "Press E to select\n\nMusic by TeknoAXE\nArt Modified from OpenGameArt", new Vector2(440, 490), Color.White);

                if (selectState == SelectState.newGame)
                {
                    spriteBatch.DrawString(menuFont, "New Game", new Vector2(440, 320), Color.Violet);
                }
                else if (selectState == SelectState.loadGame)
                {
                    spriteBatch.DrawString(menuFont, "Continue", new Vector2(440, 385), Color.Violet);
                    if (gameSaveError)
                    {
                        spriteBatch.DrawString(hintFont, "          Error Loading Save\nNo Save Data Currently Exists\n                   Press E", new Vector2(110, 370), Color.Violet);
                    }
                }
                else
                {
                    spriteBatch.DrawString(menuFont, "Exit Game", new Vector2(440, 450), Color.Violet);
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        // Clears the levels attributes
        private void levelClear()
        {
            npc.RemoveRange(0, npc.Count);
            chests.RemoveRange(0, chests.Count);
            smokeEmitter.RemoveRange(0, smokeEmitter.Count);
            door.RemoveRange(0, door.Count);
            lamp.Flicker = 5;
            screenSwipe = 1f;
        }
        // Sets new level attributes
        private void level0()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(score02);
            
            items.Add(new InventoryItems(""));
            levelClear();
            level = 0;
            player.Pos = new Vector2(150, 475);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            items.RemoveRange(0, items.Count);
            items.Add(new InventoryItems(""));
            door.Add(new DoorTrigger(new Rectangle(475, 475, 50, 100), LevelReset.level1, null, null, null));
            resetState = LevelReset.playing;
        }
        private void level1()
        {
            levelClear();
            level = 1;
            player.Pos = new Vector2(2205, 350);
            player.Velo = Vector2.Zero;
            playerCam.Pos = new Vector2(1975, 175);
            // Civilian / Sam - no hat
            npc.Add(new NpcActor(new Rectangle(300, 275, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.5f, AiState.stop, AiType.civilian, "Sams Hat", "Key"));
            items.RemoveRange(0, items.Count);
            items.Add(new InventoryItems(""));
            chests.Add(new ItemPickup(new Rectangle(500, 575, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//LampHere"), Content.Load<Texture2D>("Sprites//ItemSprites//LampGone"), new InventoryItems("Lamp")));
            chests.Add(new ItemPickup(new Rectangle(75, 575, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//SamHatHere"), Content.Load<Texture2D>("Sprites//ItemSprites//SamHatGone"), new InventoryItems("Sams Hat")));
            chests.Add(new ItemPickup(new Rectangle(1950, 350, 50, 100), Content.Load<Texture2D>("Sprites//ItemSprites//JacketHere"), Content.Load<Texture2D>("Sprites//ItemSprites//JacketGone"), new InventoryItems("Warm Jacket")));
            door.Add(new DoorTrigger(new Rectangle(1100, 525, 50, 100), LevelReset.level2, "Lamp", "Key", "I don't have everything I need"));
            resetState = LevelReset.playing;
        }
        private void level2()
        {
            levelClear();
            level = 2;
            player.Pos = new Vector2(150, 475);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            door.Add(new DoorTrigger(new Rectangle(525, 475, 50, 100), LevelReset.level3, null, null, null));
            resetState = LevelReset.playing;
        }
        private void level3()
        {
            levelClear();
            level = 3;
            player.Pos = new Vector2(425, 775);
            player.Velo = Vector2.Zero;
            playerCam.Pos = new Vector2(0, 0);
            // Brother -- Follower/ stopped
            npc.Add(new NpcActor(new Rectangle(350, 775, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.5f, AiState.stop, AiType.follower, null, null));
            // Guard -- Freindly / wondering - ESS
            npc.Add(new NpcActor(new Rectangle(1100, 400, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.5f, AiState.wonder, AiType.friendly, "Gun Ammo", "Tank Key"));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(312.5f, 825)));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item == "Radio")
                {
                    items.RemoveAt(i);
                }
                else if (items[i].Item == "Gun Ammo")
                {
                    items.RemoveAt(i);
                }
                else if (items[i].Item == "Tank Keys")
                {
                    items.RemoveAt(i);
                }
            }
            chests.Add(new ItemPickup(new Rectangle(950, 475, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//RadioHere"), Content.Load<Texture2D>("Sprites//ItemSprites//RadioGone"), new InventoryItems("Radio")));
            chests.Add(new ItemPickup(new Rectangle(125, 850, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//AmmoHere"), Content.Load<Texture2D>("Sprites//ItemSprites//AmmoGone"), new InventoryItems("Gun Ammo")));
            door.Add(new DoorTrigger(new Rectangle(575, 700, 50, 100), LevelReset.level4, "Tank Key", "Radio", "We need a Radio and Keys"));
            resetState = LevelReset.playing;
        }
        private void level4() 
        {
            levelClear();
            level = 4;
            player.Pos = new Vector2(150, 475);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            door.Add(new DoorTrigger(new Rectangle(525, 475, 50, 100), LevelReset.level5, null, null, null));
            resetState = LevelReset.playing;
        }
        private void level5()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(score03);
            levelClear();
            level = 5;
            player.Pos = new Vector2(465, 275);
            player.Velo = Vector2.Zero;
            playerCam.Pos = new Vector2(240, 23);
            //--BEAR
            npc.Add(new NpcActor(new Rectangle(100, 275, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.75f, AiState.hostile, AiType.bear, null, null));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(1162.5f, 350)));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item == "Hat")
                {
                    items.RemoveAt(i);
                }
                else if (items[i].Item == "Pickaxe")
                {
                    items.RemoveAt(i);
                }
            }
            chests.Add(new ItemPickup(new Rectangle(450, 350, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//HatHere"), Content.Load<Texture2D>("Sprites//ItemSprites//HatGone"), new InventoryItems("Hat")));
            chests.Add(new ItemPickup(new Rectangle(3900, 1150, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//PickaxeHere"), Content.Load<Texture2D>("Sprites//ItemSprites//PickaxeGone"), new InventoryItems("Pickaxe")));
            door.Add(new DoorTrigger(new Rectangle(3075, 1075, 50, 100), LevelReset.level6, "Pickaxe", null, "There's rubble in the way"));
            resetState = LevelReset.playing;
        }
        private void level6()
        {
            levelClear();
            level = 6;
            player.Pos = new Vector2(150, 475);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            door.Add(new DoorTrigger(new Rectangle(525, 475, 50, 100), LevelReset.level7, null, null, null));
            resetState = LevelReset.playing;
        }
        private void level7()
        {
            levelClear();
            level = 7;
            player.Pos = new Vector2(1525, 450);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            npc.Add(new NpcActor(new Rectangle(325, 350, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.detect, AiType.enemy, null, null));
            npc.Add(new NpcActor(new Rectangle(400, 350, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.detect, AiType.enemy, null, null));
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item == "Half Map")
                {
                    items.RemoveAt(i);
                }
                else if (items[i].Item == "Map Half")
                {
                    items.RemoveAt(i);
                }
            }
            chests.Add(new ItemPickup(new Rectangle(300, 200, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//Map01Here"), Content.Load<Texture2D>("Sprites//ItemSprites//Map01Gone"), new InventoryItems("Half Map")));
            chests.Add(new ItemPickup(new Rectangle(175, 425, 25, 25), Content.Load<Texture2D>("Sprites//ItemSprites//Map02Here"), Content.Load<Texture2D>("Sprites//ItemSprites//Map02Gone"), new InventoryItems("Map Half")));
            door.Add(new DoorTrigger(new Rectangle(75, 350, 50, 100), LevelReset.level8, "Half Map", "Map Half", "I need a map or I'll get lost"));
            resetState = LevelReset.playing;
        }
        private void level8()
        {
            levelClear();
            level = 8;
            player.Pos = new Vector2(150, 475);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            door.Add(new DoorTrigger(new Rectangle(525, 475, 50, 100), LevelReset.level9, null, null, null));
            resetState = LevelReset.playing;
        }
        private void level9()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(score04);
            levelClear();
            level = 9;
            player.Pos = new Vector2(50, 275);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            npc.Add(new NpcActor(new Rectangle(1600, 575, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.wonder, AiType.friendly, null, null));
            npc.Add(new NpcActor(new Rectangle(2050, 575, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.wonder, AiType.friendly, null, null));
            npc.Add(new NpcActor(new Rectangle(1225, 475, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.stop, AiType.civilian, null, null));
            npc.Add(new NpcActor(new Rectangle(1100, 475, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.stop, AiType.civilian, null, null));
            npc.Add(new NpcActor(new Rectangle(275, 275, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.stop, AiType.civilian, null, null));
            // dad
            npc.Add(new NpcActor(new Rectangle(850, 850, 50, 100), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnLeft02"), Content.Load<Texture2D>("Sprites//CharacterSS//AutumnFront01"), 12, 9, 1, Vector2.Zero, 1.25f, AiState.stop, AiType.civilian, null, null));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(762.5f, 900)));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(787.5f, 900)));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(787.5f, 425)));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(187.5f, 325)));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(1187.5f, 525)));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(1862.5f, 625)));
            smokeEmitter.Add(new SmokeEmitter(new Vector2(1887.5f, 625)));
            door.Add(new DoorTrigger(new Rectangle(850, 850, 50, 100), LevelReset.level10, null, null, null));
            resetState = LevelReset.playing;
        }
        private void level10()
        {
            levelClear();
            level = 10;
            player.Pos = new Vector2(150, 475);
            player.Velo = Vector2.Zero;
            playerCam.Pos = Vector2.Zero;
            door.Add(new DoorTrigger(new Rectangle(0, 0, 800, 600), LevelReset.end, null, null, null));
            resetState = LevelReset.playing;
        }
        // return the game to the main menu though resets
        private void endGame()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(score01);
            resetState = LevelReset.playing;
            selectState = SelectState.newGame;
            gameState = GameState.mainmenu;
        }
    }
}
