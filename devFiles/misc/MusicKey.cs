using Godot;
using RacingThoughts.misc;
using System;
using System.Collections.Generic;
using System.Linq;

public class MusicKey
{
    public List<AudioStream> LongFragments;
    public List<AudioStream> UpDownFragments;
    public List<AudioStream> InterestFragments;
    public List<AudioStream> ConnectionFragments;

    public string Key;

    private AugmentedRandom _rand;

    public MusicKey()
    {
        _rand = RandomSingleton.GetInstance();
    }

    public void LoadLongFragments(IEnumerable<string> resources)
    {
        LongFragments = LoadFragments(resources);
    }

    private List<AudioStream> LoadFragments(IEnumerable<string> resources)
    {
        return resources
            .Select(p => GD.Load<AudioStream>(p))
            .ToList()
            ;
    }

    public void LoadUpDownFragments(IEnumerable<string> resources)
    {
        UpDownFragments = LoadFragments(resources);
    }

    public void LoadInterestFragments(IEnumerable<string> resources)
    {
        InterestFragments = LoadFragments(resources);
    }

    public AudioStream GetNext()
    {
        var index = _rand.Next(6);

        AudioStream selectedAudio;
        if (index < 3)
        {
            selectedAudio = _rand.Random(LongFragments);
            GD.Print($"{Key} long");
        }
        else if (index < 5)
        {
            selectedAudio = _rand.Random(UpDownFragments);
            GD.Print($"{Key} up down");
        }
        else
        {
            selectedAudio = _rand.Random(InterestFragments);
            GD.Print($"{Key} interest");
        }

        return selectedAudio;
    }

    public AudioStream GetConnection()
    {
        return _rand.Random(ConnectionFragments);
    }

    public void LoadConnectionFragments(IEnumerable<string> fragments)
    {
        ConnectionFragments = LoadFragments(fragments);
    }
}