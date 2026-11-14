# Genius Square Solver

I played the Genius Square game a while ago and wondered how it could be solved using computation. This project is a solver that takes any dice roll, places the 7 blockers, and fills the remaining squares with the 9 puzzle pieces by exhaustively searching every combination.

Genius Square is a head-to-head spatial puzzle game, where two players compete to be the first to fill a 6×6 board that has blocking pieces (position decided by dice rolls) with a range of shapes. Depending on the blocking pieces, some puzzles are easy with many solutions, while others have very few. The game advertises **62,208 different puzzles, all of which have a solution**.

> **Project goals:** verify that claim, discover just how many solutions each puzzle really has, and see how fast an algorithm can solve the game.

(Also, some earlier commits have incorrect messages, as I forgot to add message for individual commits rather than pushes)
---

## How It Works

The solver is a classic **backtracking search** with a few refinements that make it dramatically faster than brute force.

### 1. Representing the pieces

Each of the 9 pieces (a monomino, domino, two trominoes, and five tetrominoes) is stored as a list of `(x, y)` offsets from an anchor square at the origin. This makes a piece trivially movable: fitting it at board position `(px, py)` is just checking whether `px + offset` lands on an empty,and in-bounds square for every offset.

### 2. Precomputing orientations

Rather than rotating pieces during the search, every distinct orientation is generated once at startup (4 rotations × 2 flips), deduplicated by a "fingerprint" (the sorted list of a shape's square coordinates). For example, the L piece yields 8 distinct orientations, while the square yields only 1. These are then anchored to the first square (0,0), so the shape orientation always covers that cell the algorithm is looking to fill. The search then never mutates a piece, it only picks from pre-built arrays. This avoids an entire class of bugs and speeds the loop up considerably.

### 3. The anchor-cell strategy

The search always fills the top-left-most empty square first, and only considers placements that cover that square. Since any valid solution must cover that cell, no solution is ever missed, but the search tree shrinks enormously compared to trying every piece at every position.

### 4. Backtracking

The solver commits a placement, recurses, and if the recursion reports failure it undoes the placement and tries the next option. When the board is full, a solution has been found. When every option at some level fails, the search unwinds until an earlier choice can be changed.

## The Code Optimisation Process

From my own testing, the algorithm was able to fully solve seeds 1-100 between **4.8 and 5.0 seeds/sec**. While this makes solving single or small ranges of seeds a quick process, it would mean solving all seeds would take an estimated 3.46 hours. While in reality, you only need to solve each puzzle once and then store the answers for later use, where is the fun in that?

### Test information:
#### Hardware:
- Ryzen 7 7840HS
- Integrated graphics (Radeon 780M)
- 32GB DDR5-5600

#### Software:
- Windows
- Power mode: balanced (power modes massively change results)
- Running program from Visual Studio 2026

#### Test parameters:
- Multi-solve
- Find all solutions
- Range: seed 1-100

So I started to look for optimisations in the code, with the biggest gains obviously being in the algorithm itself. Here are some of the optimisations I made:

### 1. CalculateOrientations being called for every seed

With how I had set the Solver class up, it was calculating every orientation of the shapes for every seed when called from a multi-solve. This involved restructuring Solver, taking logic out of the constructor, and in to the new ManageSolve class, then moving parameters so that it worked as intended. This took our solving speed to between **4.9 and 5.0 seeds/sec**, an improvement but within margin of error. As this is only per seed, rather than making tree traversal faster, it is a small improvement in the grand scheme of things.
