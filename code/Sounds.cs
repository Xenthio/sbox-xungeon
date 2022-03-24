using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox
{
	public class SoundResult
	{
		public Vector3 position;
		public Vector3 prevPosition;
		public float distance;
		public bool near;
	}

	public class Sounds
	{
		//"what? sounds? this engine already has all that!"
		// yeah yeah yeah yeah yeah no, this engine has sound but this document doesn't actually play sounds, it broadcasts sounds so that NPCs react to them and stuff
		// couldn't think of another way so gonna do it how i feel like it

		private static Vector3 zeroVec3 = new Vector3();
		private static Vector3 _soundrecent;
		public static Vector3 SoundRecent
		{
			get
			{
				return _soundrecent;
			}
			set
			{
				_soundrecent = value;
			}
		}
		private static int _soundprev;
		public static int SoundPrev
		{
			get
			{
				return _soundprev;
			}
			set
			{
				_soundprev = value;
			}
		}
		public static void alertSound( Vector3 Position)
		{
			SoundRecent = Position;
			SoundPrev = Time.Tick;
		}
		/*
		public static float distance( Vector3 v1, Vector3 v2 )
		{
			float x1 = v1.x;
			float y1 = v1.y;
			float z1 = v1.z;
			float x2 = v2.x;
			float y2 = v2.y;
			float z2 = v2.z;
			float d = (float)Math.Pow( (Math.Pow( x2 - x1, 2 ) + Math.Pow( y2 - y1, 2 ) + Math.Pow( z2 - z1, 2 ) * 1.0), 0.5 );

			return d;
		}*/
		public static SoundResult isSoundInRange( Vector3 pos, float maxRange, float minRange, Vector3 lastHeard )
		{
			SoundResult result = new SoundResult();
			
			if ( SoundRecent != zeroVec3) {
				//float d = distance( pos, SoundRecent );
				float d = Vector3.DistanceBetween( pos, SoundRecent );
				//Log.Info( d );

				result.position = SoundRecent;
				result.prevPosition = SoundRecent;
				result.near = false;
				result.distance = d;
				
				if (pos == lastHeard ) {
					return result;
				}
				if (!( Math.Abs( Time.Tick - SoundPrev) < 2))
				{
					return result;
				}
				if ( d < maxRange && d > minRange )
				{

					result.near = true;
					//SoundRecent = zeroVec3;
					//return true;
				}

			}
			//SoundRecent = zeroVec3;
			return result;
		}

	}
}
