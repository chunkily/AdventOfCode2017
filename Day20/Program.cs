using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne(File.ReadAllLines("input.txt"));

            // Particles can fly through each other with enough velocity smh
            // Provide some acceleration so they overflow faster.
            //PartTwo(new string[] {
            //    "p=<-6,0,0>, v=<3,0,0>, a=<0,100,0>",
            //    "p=<-4,0,0>, v=<2,0,0>, a=<0,100,0>",
            //    "p=<-2,0,0>, v=<1,0,0>, a=<0,100,0>",
            //    "p=<-3,0,0>, v=<-1,0,0>, a=<0,100,0>"
            //});
            PartTwo(File.ReadAllLines("input.txt"));

            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            var particles = new List<Particle>();
            for (int i = 0; i < input.Length; i++)
            {
                particles.Add(new Particle(i, input[i]));
            }

            var nearest = particles
                .OrderByDescending(p => p.Acceleration.Magnitude())
                .ThenByDescending(p => p.Velocity.Magnitude())
                .ThenByDescending(p => p.Position.Magnitude())
                .Last();

            Console.WriteLine(nearest);
        }

        static void PartTwo(string[] input)
        {
            var particles = new List<Particle>();
            for (int i = 0; i < input.Length; i++)
            {
                particles.Add(new Particle(i, input[i]));
            }

            // Run until overflow happens or all particles annihilate each other
            try
            {
                while (particles.Count > 0)
                {
                    // Check for collisions
                    for (int i = 0; i < particles.Count; i++)
                    {
                        var pi = particles[i];
                        for (int j = i + 1; j < particles.Count; j++)
                        {
                            var pj = particles[j];
                            if (pi.Position.Equals(pj.Position))
                            {
                                pi.IsDestroyed = true;
                                pj.IsDestroyed = true;
                            }
                        }
                    }
                    // Remove collided
                    particles = particles.Where(p => !p.IsDestroyed).ToList();

                    // Update all particles
                    foreach (var particle in particles)
                    {
                        particle.Update();
                    }
                }
            }
            catch (OverflowException)
            {
            }

            Console.WriteLine(particles.Count + " particles remain");
        }
    }

    class Particle
    {
        public int Id { get; set; }
        public bool IsDestroyed { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }

        const string nr = @"(-?[0-9]+)";
        static readonly Regex regex = new Regex($@"^p=<{nr},{nr},{nr}>, v=<{nr},{nr},{nr}>, a=<{nr},{nr},{nr}>$");

        public Particle(int id, string desc)
        {
            Id = id;
            var match = regex.Match(desc);

            Position = new Vector3(
                Int32.Parse(match.Groups[1].Value),
                Int32.Parse(match.Groups[2].Value),
                Int32.Parse(match.Groups[3].Value));
            Velocity = new Vector3(
                Int32.Parse(match.Groups[4].Value),
                Int32.Parse(match.Groups[5].Value),
                Int32.Parse(match.Groups[6].Value));
            Acceleration = new Vector3(
                Int32.Parse(match.Groups[7].Value),
                Int32.Parse(match.Groups[8].Value),
                Int32.Parse(match.Groups[9].Value));
        }

        public void Update()
        {
            checked
            {
                Velocity.X += Acceleration.X;
                Velocity.Y += Acceleration.Y;
                Velocity.Z += Acceleration.Z;
                Position.X += Velocity.X;
                Position.Y += Velocity.Y;
                Position.Z += Velocity.Z;
            }
        }

        public override string ToString()
        {
            return $"{Id} p={Position}, v={Velocity}, a={Acceleration}";
        }
    }

    class Vector3 : IEquatable<Vector3>
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int Magnitude()
        {
            return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        }

        public bool Equals(Vector3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override string ToString()
        {
            return $"<{X},{Y},{Z}>";
        }
    }

}
