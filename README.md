# 2d Boids

<img src="boidsv1.gif"/>

My 2d Boids implementation in Unity. Boids is swarm (or herd) algorithm which is aiming with creating bird like behavior, with set of simple rules. Every boid is separate element which is doing its own computation to find its place and direction. Every boid has range and angle of sight. Algorithm uses only three rules:

- Separation:
  which keeps boid from collisions with others members of swarm. Basically it is negative average vector of all being in range of sight other boids. 
 
- Allignment:
  boid is trying to fly in the same direction like the rest of the boids in the range of sight. It is average direction vector of all boids in the range of sight. 
  
- Cohesion:
  boid is flying towards center of "mass" created by all boids in the range of sight. It is average position of all boids in the range of sight.
  
These three rules create the opposing forces which are steering boid among others boids. It allows to gather boids in the herd and avoid the collision.

The source of algorithm:
https://www.red3d.com/cwr/boids/
