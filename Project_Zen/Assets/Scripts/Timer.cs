using UnityEngine;
using System.Collections;

/// <summary>
/// A timer
/// </summary>
public class Timer
{
    #region Fields

	bool running = false;   // Whether or not the timer is running
	float totalSeconds;     // The total seconds the timer will run
	float elapsedSeconds;   // The elapsed seconds since the timer started
	
	TimerFinishedEvent timerFinishedEvent;  // The event for when the timer finishes

    #endregion

    #region Constructors

    /// <summary>
	/// Constructor
	/// </summary>
	/// <param name="seconds">the seconds for the timer</param>
	public Timer(float seconds)
	{
        // Sets the time and creates the finish event
		this.totalSeconds = seconds;
		timerFinishedEvent = new TimerFinishedEvent();
	}

    #endregion

    #region Properties

    /// <summary>
	/// Gets or sets whether or not the timer is running 
	/// </summary>
	public bool IsRunning
	{
		get { return running; }
        set { running = value; }
	}

    /// <summary>
    /// Gets or sets the total seconds the timer will run for
    /// </summary>
    public float TotalSeconds
    {
        get { return totalSeconds; }
        set { totalSeconds = value; }
    }

    /// <summary>
    /// Gets the elapsed time
    /// </summary>
    public float ElapsedSeconds
    {
        get { return elapsedSeconds; }
    }

    #endregion

    #region Public Methods

    /// <summary>
	/// Updates the timer
	/// </summary>
	public void Update()
	{
		// Checks if the timer is running
        if (running)
		{
            // Adds to the elapsed time
			elapsedSeconds += Time.deltaTime;

            // Checks if the timer finished
			if (elapsedSeconds >= totalSeconds)
			{ Finish(); }
		}
	}
	
	/// <summary>
	/// Starts the timer
	/// </summary>
	public void Start()
	{
        // Turns on the timer and resets the elapsed time
		running = true;
		elapsedSeconds = 0;
	}

    /// <summary>
    /// Finishes the timer
    /// </summary>
    public void Finish()
    {
        // Turns off the timer and fires the finished event
        running = false;
        timerFinishedEvent.FireEvent();
    }
	
	/// <summary>
	/// Registers the provided event handler for the timer end event
	/// </summary>
	/// <param name="eventHandler">the event handler</param>
	public void Register(TimerFinishedEventHandler eventHandler)
	{
		timerFinishedEvent.Register(eventHandler);
    }

    #endregion
}
