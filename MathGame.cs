 /* 
 * Play Class
 * 
 * This class contains the Main method that runs Math Quest!
 * 
 * Author:	Patricia Bailey - CMPT322 - 2014 - Dharma Initiative 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame
{
	public class Play
	{
		static void Main()
		{
			MathQuestGame mathGame = new MathQuestGame("Bryan");

			mathGame.playTraining();
			mathGame.playMainGame();
			
			Console.ReadLine();
		}
	}
}

/* 
 * GamePlay Class
 * 
 * This class consists of the variables and methods required to play Math Quest!
 * 
 * Methods include: getPercentageCorrect(), getNum(), additionProblem(), subtractionProblem(), 
 *		setCorrectAnswer(), setIncorrectAnswers(), getRandomDigit(), playTraining().
 * 
 * Author:	Patricia Bailey - CMPT-322-01 - 2014 - Dharma Initiative 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame
{
	public class MathQuestGame
	{
		///////////////////////// Variables /////////////////////////

		private string playerName; //retrieve from database or login? initialize in constructor
		private int healthPoints;  //used in playMainGame
		private int bonusHealthPoints;  //initialize in playTraining - adds to HP 
		private int level;  //initialized in constructor

		private int numberRight = 0;  //used in playMainGame
		private int numberWrong = 0;  //used in playMainGame
		private int totalProblems = 0;  //used in playMainGame
		private int correctAnswer;  //reset after training and after every question
		private int[] choices = new int[5]; //used in playMainGame

		private const int trainingIterations = 5;
		private const int mainGameIterations = 5;

		private int bossHealth;  //used in playMainGame
		private bool attack;  //used in playMainGame - True = attack; False = defend
		private bool gameOver = false;
		private static Random random = new Random();

		//because no magic numbers...
		private const int zero = 0; 
		private const int one = 1;
		private const int two = 2;
		private const int three = 3;
		private const int four = 4;
		private const int five = 5;
		private const int ten = 10;
		private const int twenty = 20;
		private const int oneHundred = 100;

		/////////////////////// Constructor ///////////////////////

		public MathQuestGame(string studentName)
		{
			playerName = studentName;
			healthPoints = ten;
			bonusHealthPoints = zero;
			level = zero;
		}

		///////////////////////// Methods /////////////////////////

		/* Method that calculates the percentage correct in a game. 
		 * Returns a double. */
		public double getPercentageCorrect()
		{
			return Math.Round((((double)numberRight/totalProblems) * oneHundred), two);
		}

		public void getFinalResults() { }

		/* Method that generates a random number depending on the level. 
		 * Takes an int. Returns and int. */
		private int getNum(int level)
		{
			int number = zero;
			int a, b, c;

			if (level == zero){	//generates 3-digit numbers containing 3 unique digits 
					a = random.Next(one, ten); 
					do
					{
						b = random.Next(one, ten); 
					} while (a == b); 
					do 
					{
						c = random.Next(one, ten);
					} while (a == c || b == c);
					number = (a * oneHundred) + (b * ten) + c;
			}
			else {
				if (level < 5) // level one: 0-9; two: 0-29; three: 0-49; four: 0-69; five(BOSS): 0-99
					number = random.Next(zero, ((level * ten) + (ten * (level - one))));
				else
					number = random.Next(zero, (five * twenty));
			}
			return number;
		}

		/* Helper Method for playTraining that returns a random digit from a 3-digit number and the 
		 * place that the number occurs.
		 * Values returned are: int{random digit, place}(place: 0 = Hundreds, 1 = Tens, 2 = Ones)*/
		private int[] getRandomDigit(int threeDigitNumber)
		{
			int num = threeDigitNumber;
			int singleDigit;

			int[] digits = new int[three];
			for (int i = two; i >= zero; i--)
			{
				digits[i] = (int)(num % ten);
				num /= ten;
			}
			int a = random.Next(zero, three);
			singleDigit = digits[a];
			int[] result = { singleDigit, a };
			return result;
		}

		/* Method that generates integers for an addition problem dependng on the level. 
		 * Values returned are: int[0] + int[1] = int[2].
		 * Takes an int. Returns and int[]. */
		private int[] additionProblem(int level)
		{
			int a = getNum(level);
			int b = getNum(level);
			int c = a + b;		// answer
			int[] values = new int[three];

			values[zero] = a;
			values[one] = b;
			values[two] = c;	// answer

			correctAnswer = c;	// sets the correct answer
			setChoices();		// generates 3 incorrect and 1 correct answer in an int[]
			return values;
		}

		/* Method that generates integers for a subtraction problem dependng on the level. 
		 * Includes error-checking for negative numbers.
		 * int[] values returned are: int[0] - int[1] = int[2].
		 * Takes an int. Returns and int[]. */
		private int[] subtractionProblem(int level)
		{
			int a = getNum(level);
			int b = getNum(level);
			int c;
			int[] values = new int[four];

			if (a > b)
			{
				c = a - b;
				values[zero] = a;
				values[one] = b;
				values[two] = c;	// answer
			}
			else
			{
				c = b - a;
				values[zero] = b;	
				values[one] = a;
				values[two] = c;	// answer
			}
			correctAnswer = c;		// sets the correct answer
			setChoices();			// generates 3 incorrect and 1 correct answer in an int[]
			return values;
		}

		/* Method that selects three numbers that are not equal to correctAnswer
		 * to be used for choices in answering an equation */
		private void setChoices()
		{
			// generates 3 #s close to but not equal to the correct answer 
			int[] incorrectAnswers = new int[4]; 

			for (int i = zero; i < incorrectAnswers.Length; i++)
			{
				if (random.NextDouble() > 0.5)
					incorrectAnswers[i] = correctAnswer + random.Next(one, four);
				else
					incorrectAnswers[i] = correctAnswer - random.Next(one, four);
			}
			// fills choices array with correct/incorrect answers, putting correct in a random position 
			int randomPlace = random.Next(one, five);
			int j = 0;
			for (int i = 0; i < choices.Length; i++) 
			{
				if (i == randomPlace)
					choices[i] = correctAnswer;
				else
				{
					choices[i] = incorrectAnswers[j];
					j++;
				}
			}
		}
		/* Method that prints out an addition question if the player is attacking 
		 * or a subtraction question if the player is defending.
		 * Takes an int[] returned by additionProblem() or subtractionProblem()
		 * Returns a string.
		 */
		public string askQuestion(int[] questionValues)
		{
			string question = "";
			if (attack)					// attack - addition
				question = "What is " + questionValues[0] + " + " + questionValues[1] + "\n";
			else                        // defend - subtraction
				question = "What is " + questionValues[0] + " - " + questionValues[1] + "\n";
			return question;
		}
		
		/* Method that runs the Number Recognition Game. */
		public void playTraining()
		{
			Console.WriteLine("\n" + playerName + "! To begin your quest, you must first complete your training!\n");
			Console.WriteLine("In order to defeat the evil Math Magician and rescue River from the dungeons\n");
			Console.WriteLine("you need to know your numbers!!!\n");
			Console.WriteLine("Pick the correct place - Ones, Tens, or Hundreds - to earn extra lives!!\n\n");

			for (int i = zero; i < trainingIterations; i++)
			{
				int num = getNum(level);				// level should be 0, so getNum() generates a random 3-digit #
				int[] digit = getRandomDigit(num);		// takes 3-digit #, and returns a random # from it and its place 
				string correct;							// will hold correct place

				if (digit[one] == zero)					//hundreds
					correct = "c";
				else if (digit[one] == one)				//tens
					correct = "b";
				else
					correct = "a";						//ones
				
				//Question is asked
				Console.WriteLine("\nYour number is " + num + "\n");
				Console.WriteLine("What place is " + digit[zero] + " in?\n");
				Console.WriteLine("A) Ones\nB) Tens\nC) Hundreds\n");
				Console.WriteLine("Type A, B, or C for your answer.\n");

				string input = Console.ReadLine();		//Accepts input from the user
				Console.WriteLine("\n");

				if (input.ToLower() == correct)			//CORRECT ANSWER
				{
					Console.WriteLine("Good job! That is correct!");
					if (correct == "a")
						Console.WriteLine(digit[zero] + " is in the Ones place!\n");
					else if (correct == "b")
						Console.WriteLine(digit[zero] + " is in the Tens place!\n");
					else
						Console.WriteLine(digit[zero] + " is in the Hundreds place!\n");
					bonusHealthPoints++;						//Increments bonus lives
				}
				else									//INCORRECT ANSWER
				{
					Console.WriteLine("Incorrect.");
					if (correct == "a")
						Console.WriteLine(digit[zero] + " is in the Ones place.\n");
					else if (correct == "b")
						Console.WriteLine(digit[zero] + " is in the Tens place.\n");
					else
						Console.WriteLine(digit[zero] + " is in the Hundreds place.\n");
				}
			}
			Console.WriteLine("You have earned " + bonusHealthPoints + " bonus health points!\n\n\n\n");
			correctAnswer = 0;							//resets correctAnswer
			level++;									//increments level to 1
		}

		/* Method for the Main Game play. */
		public void playMainGame() // 10 add and 10 subtract per level
		{
			healthPoints = healthPoints + bonusHealthPoints;		// add bonus points to HP

			Console.WriteLine("TEST: Welcome to the main game");

			while(level <= mainGameIterations && !gameOver)	// cycles through 5 levels
			{
				Console.WriteLine("TEST: Entering level-cycling While-loop.\n");
				bossHealth = five;
				attack = true;
				int inputValue;

				while (bossHealth > 0 && !gameOver)			// cycles through each boss
				{
					Console.WriteLine("TEST: Entering BOSS-cycling While-loop.\n");

					if (healthPoints > 0)
					{
						if (attack)
						{
							Console.WriteLine("TEST: Attack the monster!");
							Console.WriteLine(askQuestion(additionProblem(level)));// addition problem
							
							string input = Console.ReadLine();	//Accepts input from the user
							Console.WriteLine("\n");
							inputValue = Int32.Parse(input);
							Console.WriteLine("TEST input value: " + inputValue + "\n");

							if (inputValue == correctAnswer)	// correct answer
							{
								bossHealth--;
								numberRight++;
								Console.WriteLine("Correct! Monster takes damage!\n");
								Console.WriteLine("Your health: " + healthPoints + "\n");
								Console.WriteLine("Monster's health: " + bossHealth + "\n");
							}
							else                                // wrong answer
							{
								numberWrong++;
								Console.WriteLine("Incorrect! You miss!\n");
								Console.WriteLine("Your health: " + healthPoints + "\n");
								Console.WriteLine("Monster's health: " + bossHealth + "\n");
							}
							totalProblems++;
							attack = false;
						}
						else                                    // subtraction     
						{
							Console.WriteLine("TEST: Monster attacks you! Defend yourself!\n");
							Console.WriteLine(askQuestion(subtractionProblem(level)));

							string input = Console.ReadLine();	//Accepts input from the user
							Console.WriteLine("\n");
							inputValue = Convert.ToInt32(input);

							if (inputValue == correctAnswer)	// correct answer
							{
								numberRight++;
								Console.WriteLine("Correct! Monster misses!\n");
								Console.WriteLine("Your health: " + healthPoints + "\n");
								Console.WriteLine("Monster's health: " + bossHealth + "\n");
							}
							else                                // wrong answer
							{
								healthPoints--;
								numberWrong++;
								Console.WriteLine("Incorrect! You take damage!\n");
								Console.WriteLine("Your health: " + healthPoints);
								Console.WriteLine("Monster's health: " + bossHealth + "\n");
							}
							totalProblems++;
							attack = true;
						}
					}
					else
						gameOver = true;
				}	// end while (end of cycle through bosses or game over)
				if (!gameOver)
				{
					level++;
					Console.WriteLine("You have slain the monster!!");
					Console.WriteLine("New Level: " + level + "\nHealth: " + healthPoints);
				}
			}		// end while (end of cycle through levels or game over)
			if (!gameOver)
				Console.WriteLine("TEST: You won!\n");//win screen with final results
			else
				Console.WriteLine("TEST: You have failed! River is dead!\n"); //game over screen with final results
		}
				
		//public void reset();

		/* Main method for testing */
		/*static void Main()
		{
			GamePlay mathGame = new GamePlay("Bryan");		
			//mathGame.playTraining();
			//mathGame.playMainGame();
			Console.WriteLine(mathGame.getPercentageCorrect());
	
			for (int i = 0; i < 11; i++) {	  //to test the getNum() numbers
				for (int j = 0; j < 10; j++) {
					Console.WriteLine(mathGame.getNum(i));
				}
			}		
			Console.ReadLine();
		}*/ 
	}
}
