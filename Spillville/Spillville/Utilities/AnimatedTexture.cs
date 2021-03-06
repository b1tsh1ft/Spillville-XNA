#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Spillville.Utilities
{
    public class AnimatedTexture
    {
        private int framecount;
    	private Texture2D myTexture;
    	private List<Color[]> colorMasks;

    	private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;

    	public Color PaintColor;

    	public float Rotation, Width, Depth, Scale;
        public Vector2 Origin;
        public AnimatedTexture(Vector2 origin, float rotation, 
            float width, float depth)
        {
            Origin = origin;
            Rotation = rotation;
            Width = width;
            Depth = depth;

        }
        public void Load(ContentManager content, string asset, 
            int frameCount, int framesPerSec)
        {
            framecount = frameCount;
            myTexture = content.Load<Texture2D>(asset);
			colorMasks = new List<Color[]>();
			for (int j = 0; j <= 10; j++ )
			{
				Color[] bla = new Color[myTexture.Width * myTexture.Height];
				myTexture.GetData(bla);
				for (int i = 0; i < bla.Length; i++)
				{
					if (IsColorSimilar(bla[i], Color.Blue, 200, 200, 200))
					{
						bla[i] = GetColor(j);
					}
				}
				colorMasks.Add(bla);
			}
				

            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        	PaintColor = Color.White;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

		public void UpdateWaterColor(int color)
		{
			Debug.WriteLine(color);
			myTexture.SetData(colorMasks[color]);
		}

		private static bool IsColorSimilar(Color original, Color test, int redDelta, int blueDelta, int greenDelta)
		{
			return Math.Abs(original.R - test.R) < redDelta && Math.Abs(original.G - test.G) < greenDelta && Math.Abs(original.B - test.B) < blueDelta;
		}

		private static Color GetColor(int color)
		{
			switch (color)
			{
				case 0:
					return Color.Black;
				case 1:
					return new Color(0.0f, 0.0f, 32.0f, 255.0f);
				case 2:
					return new Color(0.0f, 0.0f, 64.0f, 255.0f);
				case 3:
					return new Color(0.0f, 0.0f, 96.0f, 255.0f);
				case 4:
					return new Color(0.0f, 0.0f, 128.0f, 255.0f);
				case 5:
					return new Color(0.0f, 0.0f, 160.0f,255.0f);
				case 6:
					return new Color(0.0f, 0.0f, 192.0f,220.0f);
				case 7:
					return new Color(0.0f, 0.0f, 192.0f,190.0f);
				case 8:
					return Color.Blue;
				case 9:
					return Color.Red;
				case 10:
					return Color.White;
			}
			return Color.Black;
		}

        // class AnimatedTexture
		public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }


        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = myTexture.Width / framecount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, myTexture.Height);
			if(Scale == default(float))
			{
				Scale = Width/sourcerect.Width;
			}
            batch.Draw(myTexture, screenPos, sourcerect,PaintColor,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}
