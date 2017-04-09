using UnityEngine;

namespace Merge
{
	/// <summary>
	/// 
	/// Singleton.cs
	/// 
	/// The Merge Singleton class.  Extends MonoBehaviour to enable singleton-like
	/// functionality, including global static access via the 'instance' property,
	/// enforced single instance functionality (if the 'instance' property is already
	/// set, it will not create a new one, and it will error if placed twice in a scene),
	/// and auto-creation of an object if the component is not found in the scene at all.
	/// 
	/// Each of these features can be turned on and off on a class by class basis,
	/// so for example, you can disable the error when placing multiple copies in a scene,
	/// if you really want to have multiple copies of it. (Just keep in mind the instance
	/// property will only return one of the copies).
	/// Or you can disable auto-creation of the object, in case the object needs to be 
	/// configured by hand in the scene and auto-creation is insufficient.
	/// You can also disable the setting of 'DontDestroyOnLoad', so the objects get recycled
	/// between scene loads.
	/// 
	/// In order to facilitate the optional feature-set, we use virtual functions and 
	/// overrides to enable and disable individual functionality.
	/// </summary>
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour, new()
	{
		/// <summary>
		/// This determines if the Singleton enforces only having a single instance
		/// of this component in a scene at any given time.
		/// </summary>
		protected static bool singletonEnforceSingleInstance=true;

		/// <summary>
		/// This determines if the Singleton will auto-create an object with the desired
		/// component attached, if one doesn't already exist in the scene.
		/// </summary>
		protected static bool singletonAutoCreateInstance=true;
		
		/// <summary>
		/// This determines if the Singleton's object will persist through scene loads.
		/// </summary>
		protected static bool singletonPersistInstance=true;

		/// <summary>
		/// The actual static member that holds the reference to the instance.
		/// </summary>
		protected static T instance;

		/// <summary>
		/// The public property that gates access to the 'instance' static, and performs our
		/// magic if needed.
		/// </summary>
		public static T Instance
		{
			get
			{
				// If we haven't been created yet...
				if (instance == null)
				{
					// Make sure there isn't a sneaky object in the scene already
					// (There should not be, as Awake() would have set the instance member).
					instance = (T)FindObjectOfType(typeof(T));
					if (instance == null)
					{
						// We didn't find an object with this Component on it in the Scene,
						// so we will auto-create an object, name it after the component type,
						// and add the component to the object.
						if(singletonAutoCreateInstance == true)
						{
							GameObject go = new GameObject();
							go.name = typeof(T).FullName+" (auto)";
							var comp = go.AddComponent<T>();

							if(singletonPersistInstance == true)
							{
								DontDestroyOnLoad(go);
							}

							return comp;
						}
					}
				}
				return instance;
			}
		}

		void Awake()
		{
			// If we want to enforce a single instance of this component...
			if(singletonEnforceSingleInstance == true)
			{
				if(instance != null)
				{
					// The static instance member is already set.  This means another object
					// was already created with this component, and thus multiple copies of a
					// Singleton were about to exist!

					Debug.LogError ("Error!  Singleton of type "+typeof(T).FullName+" is set to enforce a single instance and an instance already exists!");
				}
			}

			// Set the static instance member to this component.
			instance = this as T;

			// If we are set to persist our instances through scene loads,
			// call DontDestroyOnLoad on our parent GameObject.
			if(singletonPersistInstance == true)
			{
				DontDestroyOnLoad(gameObject);
			}
		}



	}

}