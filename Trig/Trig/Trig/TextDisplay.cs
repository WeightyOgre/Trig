using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Trig
{
    class TextDisplay
    {

        //text output for testing purposes
        SpriteFont Font1;

        ContentManager content;

        SpriteBatch spritebatch;

        String aString;

        Vector2 stringPosition;

        // Sets and gets the string value
        public String stringValue
        {
            get { return aString; }
            set { aString = value; } // Negative zoom will flip image
        }

        // Sets and gets the string position
        public Vector2 stringPositionValue
        {
            get { return stringPosition; }
            set { stringPosition = value; } // Negative zoom will flip image
        }

        public TextDisplay(ContentManager content, String aString, Vector2 stringPosition)
        {
            this.content = content;
            Font1 = content.Load<SpriteFont>("SpriteFont1");
            this.aString = aString;
            this.stringPosition = stringPosition;
        }

        public void DrawFont(SpriteBatch spritebatch)
        {
            this.spritebatch = spritebatch;
            spritebatch.Begin();
            spritebatch.DrawString(Font1,
            aString,
            stringPosition,
            Color.Black);
            spritebatch.End();
        }

    }
}
