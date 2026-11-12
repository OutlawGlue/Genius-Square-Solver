# Genius Square Solver

I played the Genius Square game a while ago and wondered how it could be solved using computation. This project is a solver that takes any dice roll, places the 7 blockers, and fills the remaining squares with the 9 puzzle pieces by exhaustively searching every combination.

Genius Square is a head-to-head spatial puzzle game, where two players compete to be the first to fill a 6×6 board that has blocking pieces (position decided by dice rolls) with a range of shapes. Depending on the blocking pieces, some puzzles are easy with many solutions, while others have very few. The game advertises **62,208 different puzzles, all of which have a solution**.

> **Project goals:** verify that claim, discover just how many solutions each puzzle really has, and see how fast an algorithm can solve the game.

---

## How it works

The solver is a classic **backtracking search** with a few refinements that make it dramatically faster than brute force.

### 1. Representing the pieces

Each of the 9 pieces (a monomino, domino, two trominoes, and five tetrominoes) is stored as a list of `(x, y)` offsets from an anchor square at the origin. This makes a piece trivially movable: fitting it at board position `(px, py)` is just checking whether `px + offset` lands on an empty,and in-bounds square for every offset.

### 2. Precomputing orientations

Rather than rotating pieces during the search, every distinct orientation is generated once at startup (4 rotations × 2 flips), deduplicated by a "fingerprint" (the sorted list of a shape's square coordinates). For example, the L piece yields 8 distinct orientations, while the square yields only 1. The search then never mutates a piece, it only picks from pre-built arrays. This avoids an entire class of bugs and speeds the loop up considerably.

### 3. The anchor-cell strategy

The search always fills the top-left-most empty square first, and only considers placements that cover that square. Since any valid solution must cover that cell, no solution is ever missed, but the search tree shrinks enormously compared to trying every piece at every position.

### 4. Backtracking

The solver commits a placement, recurses, and if the recursion reports failure it undoes the placement and tries the next option. When the board is full, a solution has been found. When every option at some level fails, the search unwinds until an earlier choice can be changed.