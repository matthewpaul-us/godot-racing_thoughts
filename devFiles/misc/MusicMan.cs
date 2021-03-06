using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class MusicMan : Node
{
	[Export] public List<string> CAmbience;
	[Export] public List<string> EAmbience;
	[Export] public List<string> GAmbience;
	[Export] public List<string> AAmbience;
	[Export] public List<string> ChordProgression;

	public AudioStreamPlayer AmbienceTrack1;
	public AudioStreamPlayer AmbienceTrack2;
	public AudioStreamPlayer ConnectionTrack1;
	public AudioStreamPlayer ConnectionTrack2;
	public AudioStreamPlayer ConnectionTrack3;
	public AudioStreamPlayer StingerTrack;

	private Timer _timer;
	private Tween _tween;
	private MusicKey CKey;
	private MusicKey EKey;
	private MusicKey GKey;
	private MusicKey AKey;

	public MusicKey[] Keys { get; private set; }

	private int _progressionIndex;
	private int _ambientTrackIndex;
	private int _connectionTrackIndex;
	private AudioStreamPlayer[] _connectionTracks;

	private bool _isPlaying;
	private AugmentedRandom _rand;

	public override void _Ready()
	{
		AmbienceTrack1 = GetNode<AudioStreamPlayer>("AmbienceTrack");
		AmbienceTrack2 = GetNode<AudioStreamPlayer>("AmbienceTrack2");
		ConnectionTrack1 = GetNode<AudioStreamPlayer>("ConnectionTrack");
		ConnectionTrack2 = GetNode<AudioStreamPlayer>("ConnectionTrack2");
		ConnectionTrack3 = GetNode<AudioStreamPlayer>("ConnectionTrack3");
		StingerTrack = GetNode<AudioStreamPlayer>("JingleTrack");
		_timer = GetNode<Timer>("Timer");
		_tween = GetNode<Tween>("Tween");

		_ambienceTracks = new []{ AmbienceTrack1, AmbienceTrack2};
		_connectionTracks = new []{ ConnectionTrack1, ConnectionTrack2, ConnectionTrack3};
		_ambientTrackIndex = 0;
		_progressionIndex = 0;
		_connectionTrackIndex = 0;
		_rand = RandomSingleton.GetInstance();

		CKey = ProcessAmbience("C", CAmbience);
		EKey = ProcessAmbience("E", EAmbience);
		GKey = ProcessAmbience("G", GAmbience);
		AKey = ProcessAmbience("A", AAmbience);

		Keys = new[] { CKey, EKey, GKey, AKey };
	}

	public void SetIsPlaying(bool isPlaying)
	{
		if (!_isPlaying)
		{
			OnTimerTimeout();
			FadeIn(1.5f);
			_timer.Start();
		}
		if (_isPlaying)
		{
			FadeOut(3f);
			_timer.Stop();
		}
	}

	private void FadeOut(float timeToFade)
	{
		foreach (var player in _ambienceTracks.Concat(_connectionTracks))
		{
			_tween.InterpolateProperty(player, "volume_db", player.VolumeDb, -60f, timeToFade);
		}

		_tween.Start();
	}

	private void FadeIn(float timeToFade)
	{
		foreach (var player in _ambienceTracks.Concat(_connectionTracks))
		{
			_tween.InterpolateProperty(player, "volume_db", -60, 0f, timeToFade);
		}

		_tween.Start();
	}

	public MusicKey ProcessAmbience(string key, IEnumerable<string> fragments)
	{
		var musicKey = new MusicKey();
		musicKey.Key = key;
		var longFragments = fragments
			.Where(p => p.Contains("long"))
			;

		var upDownFragments = fragments
			.Where(p => p.Contains("up_down"))
			;

		var interestFragments = fragments
			.Where(p => p.Contains("interest"))
			;

		var connectionFragments = fragments
			.Where(p => p.Contains("connection"))
			;

		var successFragments = fragments
			.Where(p => p.Contains("success"));
            ;

		var failureFragments = fragments
			.Where(p => p.Contains("failure"));

		musicKey.LoadLongFragments(longFragments);
		musicKey.LoadUpDownFragments(upDownFragments);
		musicKey.LoadInterestFragments(interestFragments);
		musicKey.LoadConnectionFragments(connectionFragments);
		musicKey.LoadSuccessFragments(successFragments);
		musicKey.LoadFailureFragments(failureFragments);

		return musicKey;
	}

	private AudioStreamPlayer[] _ambienceTracks;

	public MusicKey GetCurrentKey()
	{
		MusicKey currentKey;
		string currentProgression = ChordProgression.ElementAt(_progressionIndex);
		switch (currentProgression)
		{
			case "C":
				currentKey = CKey;
				break;
			case "E":
				currentKey = EKey;
				break;
			case "G":
				currentKey = GKey;
				break;
			case "A":
				currentKey = AKey;
				break;
			default:
				currentKey = CKey;
				break;
		}

		return currentKey;
	}

	public void OnTimerTimeout()
	{
		MusicKey currentKey = GetCurrentKey();

		_ambienceTracks[_ambientTrackIndex].Stream = currentKey.GetNext();
		_ambienceTracks[_ambientTrackIndex].Play();

		_progressionIndex = (_progressionIndex + 1) % ChordProgression.Count();
		_ambientTrackIndex = (_ambientTrackIndex + 1) % _ambienceTracks.Count();
	}

	public void PlayConnection()
	{
		_connectionTracks[_connectionTrackIndex].Stream = GetCurrentKey().GetConnection();
		_connectionTracks[_connectionTrackIndex].Play();


		_connectionTrackIndex = (_connectionTrackIndex + 1) % _connectionTracks.Count();
	}

	public void PlaySuccess()
	{
		MusicKey currentKey = GetCurrentKey();

		if(!currentKey.SuccessFragments.Any())
		{
			currentKey = _rand.Random(Keys.Where(k => k.SuccessFragments.Any()).ToList());
		}

        StingerTrack.Stream = currentKey.GetSuccess();
		StingerTrack.Play();
	}

	public void PlayFailure()
	{
		MusicKey currentKey = GetCurrentKey();

		if(!currentKey.FailureFragments.Any())
		{
			var keysWithFragment = Keys.Where(k => k.FailureFragments.Any()).ToList();
			currentKey = _rand.Random(keysWithFragment);
		}

		StingerTrack.Stream = currentKey.GetFailure();
		StingerTrack.Play();
	}
}
