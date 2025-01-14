### Expected output

Here is some pseudo code showing the expected interaction and one way to
represent the output (you are encouraged to find a better way).

You can use a command line output, work in the browser or use an electric
circuit.

Use whatever programming language you want, as long as we can execute your
solution.

```
// create a new song
song = Song -> bpm: 128, title: Animal Rights

// add the 3 patterns
song <- Kick   |X|_|_|_|X|_|_|_|
song <- Snare  |_|_|_|_|X|_|_|_|
song <- HiHat  |_|_|X|_|_|_|X|_|

song.play

// -> real time output
// |Kick|_|HiHat|_|Kick+Snare|_|HiHat|_|Kick|_|HiHat|.....
```

![Output example](/drummachine-kata.gif?raw=true)

(ignore the audio outputs print at the beginning but pay attention to
the output speed at a BPM of 128)

### Extra info

* A song contains multiple patterns being sequenced for different
  samples.
* A song plays at a given tempo (AKA bpm), the tempo does not need to be able
  change while the song plays.
* For this exercise, you are expected to implement 3 patterns for the following
  sounds/samples: kick, snare and hihat (you can use the example pattern or
  come up with your own).
* The time signature is expected to be 4/4 (if you don't know what that is,
  don't worry and ignore this instruction).
* The pattern is expected to be 8 steps or more.
* Your code will be executed on the command line or in the browser.
* Try to keep external dependencies to a minimum.
* See next section to see what to output.
* The output isn't expected to be in sync with the tempo/BPM, but if you'd like
  to do so, go for it!


### Timing information

Outputting the patterns in tempo isn't required for this exercise, but if you
wish to do so you might need some extra information.

At a 4/4 time signature of 60 BPM (beats per minute), we get 1 beat per second.
We can assume that 8 steps = 1 bar, representing 4 beats.

In other words, a 8 step pattern would take `(60/BPM)*4` seconds to play and
each step would take `((60/BPM)*4)/8` seconds.


### If you're having fun

**If you're submitting this exercise to apply for a job at Splice, please know
that under no circumstances are any of these things expected or required. These
are just some ideas for extending your work, if you're having fun with the
project!**

* Try mixing and matching patterns of different durations (8, 16, 32 steps),
  note that if you have 2 patterns, one 8 and one 16, the 8 should play
  twice while the 16 plays once.
* Add support for velocity (the amplitude/volume of a note).
* Try to output sound (macOS has the `say` command, Windows has `ptts`,
  also most language have bindings for [portaudio](http://www.portaudio.com/)

If you can't stop:

* How about live play? Can you allow users to add/remove/change patterns
  while playing?
* Make it even more interactive and add a web interface/GUI!


### Splice Evaluation

If you are given this exercise as a code challenge, we are going to anonymize
your submission and evaluate it using a rubric. There is no specific language
requirement for the submission. **Please use the language with which you are
most comfortable working, as we'd like to see how you'd approach something when
working with familiar tools.** Provided things look good, we'll set up a time to
meet with two backend engineers to chat about the approach you took, and answer
any questions you may have.

So that you're prepared, here is an overview of the things we'll be evaluating
and discussing about your submission:

* How much time did you spend on the exercise? What parts took longer?
* What were the hard parts, and what parts did you enjoy most?
* How thoroughly is the submission documented?
* How well is the submission tested?
* How clearly are the key concepts modeled?
* What kinds of trade-offs were made in balancing simplicity and flexibility?
  Can your solution be easily extended to support a change in requirements?
* Is the code written in a readable and consistent style, such that someone
  familiar with the language would understand what you're doing easily?

### Submit your solution

If you want your solution to be reviewed you can:

* email the hiring manager your implementation as a zip or link to your repo
  (for candidates we are interviewing only)
* fork this repo and send a PR (community reviewed)
* create an issue with the url for your implementation (community reviewed)
