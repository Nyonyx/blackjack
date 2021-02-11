using System;
using System.Collections.Generic;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GCMonogame
{
    public class gameSettings{
        public bool is_setting_open {get;private set;}

        private Button tableRight;
        private Button tableLeft;
        private Button settingsButton;
        private Sprite backgroundUI;

        SceneGameplay board;

        private int tableIndex = 0;
        private int leatherIndex = 0;
        TextLabel textTable;
        TextLabel textSettings;


        public gameSettings(MainGame pMainGame,SceneGameplay pBoard){
            this.board = pBoard;
            is_setting_open = false;

            settingsButton = new Button(pMainGame.Content.Load<Texture2D>("UI/settingsBtn"),"",new Vector2(GameState.Screen.Width-100,20),toggle);
            settingsButton.scaling = new Vector2(0.2f,0.2f);
           

            backgroundUI = new Sprite(pMainGame.Content.Load<Texture2D>("UI/blueBG"));
            backgroundUI.Position = new Vector2(GameState.Screen.Width/2,GameState.Screen.Height/2);
            backgroundUI.origin = new Vector2(backgroundUI.Texture.Width/2,backgroundUI.Texture.Height/2);
            backgroundUI.scaling = new Vector2(1.5f,1.5f);


            tableRight = new Button(pMainGame.Content.Load<Texture2D>("UI/button1"),"",new Vector2(backgroundUI.Position.X + 100,backgroundUI.Position.Y - 70),changeTable);
            tableLeft = new Button(pMainGame.Content.Load<Texture2D>("UI/button1"),"",new Vector2(backgroundUI.Position.X - 100,backgroundUI.Position.Y - 70),changeTable);
            tableRight.scaling = new Vector2(0.3f,0.3f);
            tableLeft.scaling = new Vector2(0.3f,0.3f);

            tableRight.isActive = false;
            tableLeft.isActive = false;
            backgroundUI.isActive = false;
            tableRight.zOrder = 2;
            tableLeft.zOrder = 2;
            backgroundUI.zOrder = 1;

            Sprite iconArrowR = new Sprite(pMainGame.Content.Load<Texture2D>("UI/backBtn"));
            iconArrowR.origin = new Vector2(iconArrowR.Texture.Width/2,iconArrowR.Texture.Height/2);
            iconArrowR.scaling = new Vector2(0.2f,0.2f);

            Sprite iconArrowL = new Sprite(pMainGame.Content.Load<Texture2D>("UI/backBtn"));
            iconArrowL.origin = new Vector2(iconArrowR.Texture.Width/2,iconArrowR.Texture.Height/2);
            iconArrowL.scaling = new Vector2(0.2f,0.2f);

            iconArrowL.setSpriteEffect(SpriteEffects.FlipHorizontally);
            tableRight.setIcon(iconArrowR);
            tableLeft.setIcon(iconArrowL);

            textTable = new TextLabel("Table");
            textTable.Position = new Vector2(GameState.Screen.Width/2,GameState.Screen.Height/2);
            textTable.isCenter = true;
            textTable.zOrder = 2;
            textTable.isActive = false;

            textSettings = new TextLabel("Settings");
            textSettings.isCenter = true;
            textSettings.Position = new Vector2(backgroundUI.Position.X,backgroundUI.Position.Y - backgroundUI.Texture.Height + 75);
            textSettings.zOrder = 2;
            textSettings.isActive = false;

            board.addActor(settingsButton);
            board.addActor(backgroundUI);
            board.addActor(tableRight);
            board.addActor(tableLeft);
            board.addActor(textTable);
            board.addActor(textSettings);
            
        }

        private void toggle(Button pSender){
            if (!is_setting_open){
                is_setting_open = true;
                tableRight.isActive = true;
                tableLeft.isActive = true;
                backgroundUI.isActive = true;
                textTable.isActive = true;
                textSettings.isActive = true;
            }else{
                is_setting_open = false;
                tableRight.isActive = false;
                tableLeft.isActive = false;
                backgroundUI.isActive = false;
                textTable.isActive = false;
                textSettings.isActive = false;
            }
        }
        
        private void changeTable(Button pButton){
            if (pButton == tableRight){
                tableIndex +=1;
            }else if(pButton == tableLeft){
                tableIndex -=1;
            }
            if (tableIndex > AssetManager.table.Length-1){
                tableIndex = 0;
            }else if(tableIndex < 0){
                tableIndex = AssetManager.table.Length-1;
            }
    
            board.table.setTexture(AssetManager.table[tableIndex]);
        }
        private void changeLeather(Button pButton){

        }
        private void changeBackground(Button pButton){

        }
        
    }
}