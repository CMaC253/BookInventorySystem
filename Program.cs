using System;
using System.Collections.Generic;
using System.Text;
//Colin McDonald 
//Assignment 3 (revision)
//BIT 143 Winter 2016


    //EXTRA CREDIT ANSWER:

//The reason why the program would not work is because
//the allTests class was never implemented in this file set
//therefore we get an assembly reference compile-time error
//the fix came quick because the only option was to comment out
//I put a writeLine stating that the tests dont work just in case
//I/or the user calls it. In the workplace I would send an e-mail about it
//after commenting out the issue, so I could escalate the issue
//and I would continue working without the tests, because
//going back through and creating that class would be time inefficient.
   
namespace MulitList_Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            (new UserInterface()).RunProgram();

            // Or, you could go with the more traditional:
            // UserInterface ui = new UserInterface();
            // ui.RunProgram();
        }
    }

    // Bit of a hack, but still an interesting idea....
    enum MenuOptions
    {
        // DO NOT USE ZERO!
        // (TryParse will set choice to zero if a non-number string is typed,
        // and we don't want to accidentally set nChoice to be a member of this enum!)
        QUIT = 1,
        ADD_BOOK,
        PRINT_BY_AUTHOR,
        PRINT_BY_TITLE,
        REMOVE_BOOK,
        RUN_TESTS
    }

    class UserInterface
    {
        MultiLinkedListOfBooks theList;
        public void RunProgram()
        {
            int nChoice;
            theList = new MultiLinkedListOfBooks();

            do // main loop
            {
                Console.WriteLine("Your options:");
                Console.WriteLine("{0} : End the program", (int)MenuOptions.QUIT);
                Console.WriteLine("{0} : Add a book", (int)MenuOptions.ADD_BOOK);
                Console.WriteLine("{0} : Print all books (by author)", (int)MenuOptions.PRINT_BY_AUTHOR);
                Console.WriteLine("{0} : Print all books (by title)", (int)MenuOptions.PRINT_BY_TITLE);
                Console.WriteLine("{0} : Remove a Book", (int)MenuOptions.REMOVE_BOOK);
                Console.WriteLine("{0} : RUN TESTS", (int)MenuOptions.RUN_TESTS);
                if (!Int32.TryParse(Console.ReadLine(), out nChoice))
                {
                    Console.WriteLine("You need to type in a valid, whole number!");
                    continue;
                }
                switch ((MenuOptions)nChoice)
                {
                    case MenuOptions.QUIT:
                        Console.WriteLine("Thank you for using the multi-list program!");
                        break;
                    case MenuOptions.ADD_BOOK:
                        this.AddBook();
                        break;
                    case MenuOptions.PRINT_BY_AUTHOR:
                        theList.PrintByAuthor();
                        break;
                    case MenuOptions.PRINT_BY_TITLE:
                        theList.PrintByTitle();
                        break;
                    case MenuOptions.REMOVE_BOOK:
                        this.RemoveBook();
                        break;
                    case MenuOptions.RUN_TESTS:
                        Console.WriteLine("Tests not availalbe");
                        
                        break;
                    default:
                        Console.WriteLine("I'm sorry, but that wasn't a valid menu option");
                        break;

                }
            } while (nChoice != (int)MenuOptions.QUIT);
        }

        public void AddBook()
        {
            Console.WriteLine("ADD A BOOK!");

            Console.WriteLine("Author name?");
            string author = Console.ReadLine();

            Console.WriteLine("Title?");
            string title = Console.ReadLine();

            double price = -1;
            while (price < 0)
            {
                Console.WriteLine("Price?");
                if (!Double.TryParse(Console.ReadLine(), out price))
                {
                    Console.WriteLine("I'm sorry, but that's not a number!");
                    price = -1;
                }
                else if (price < 0)
                {
                    Console.WriteLine("I'm sorry, but the number must be zero, or greater!!");
                }
            }

            ErrorCode ec = theList.Add(author, title, price);
            if(ec == ErrorCode.DuplicateBook)
                Console.WriteLine("Duplicate book found. Entry failed!");
            if(ec== ErrorCode.OK)
                Console.WriteLine("Book was successfully entered.");

        }

        public void RemoveBook()
        {
            Console.WriteLine("REMOVE A BOOK!");

            Console.WriteLine("Author name?");
            string author = Console.ReadLine();

            Console.WriteLine("Title?");
            string title = Console.ReadLine();

            ErrorCode ec = theList.Remove(author, title);

            if (ec == ErrorCode.BookNotFound)
                Console.WriteLine("Book was searched for, but not found. Deletion failed!");
            if (ec == ErrorCode.OK)
                Console.WriteLine("Book was successfully removed.");
        }
    }

    enum ErrorCode
    {
        OK,
        DuplicateBook,
        BookNotFound
    }

    class MultiLinkedListOfBooks
    {
        private class Book
        {
            public Book tNext;
            public Book aNext;
            public string author;
            public string title;
            public double price;

            public Book(string a, string t, double p)
            {
                author = a;
                title = t;
                price = p;
            }
            //In print we want to pass a book through, or just use this.
            //to get the data fields for each node. Notice the pro C# 6.0
            //use of $ before the quote. I really like that feature.
            public void Print(Book parBook)
            {
                
                Console.WriteLine($"Author: {parBook.author} \n" +
                                   $"Title: {parBook.title}   \n" +
                                   $"Price: {parBook.price}   \n");

            }

            public int CompareByAuthor(Book otherBook) 
            {
                if (this.author == otherBook.author && this.title == otherBook.title) //first and foremost we need 
                    return 0;                                                         //to check for a duplicate

                if (this.author == otherBook.author)                         //if this author is same as the parameter then 
                    return string.Compare(this.title, otherBook.title);      //compare the title 

                return string.Compare(this.author, otherBook.author);       //If the other two conditions fail, the only other option
                                                                            //is to use the string.Compare method that was a lot easier
                                                                            //to understand still returns negative if it string A precedes B
                                                                            //and positive if A follows B, we use this. to reference the object calling 
                                                                            //the compare method


                //I realized that originally I was returning the value of -1,0 and 1 for compareTo when it returns less than one
                //greater than one or 0 instead of relying on the string.Compare method, which does that for us
                //I tried to work through my original logic but was compromised when adding
                //this logic in the while loops in  the add method. Upon more research I
                //found what I needed to change, and implemented a more correct method because I still needed to compare
                //the title if the author was the same and vice versa for if the title was the same.
            }


            public int CompareByTitle(Book otherBook)
            {//Same thing here

                if (this.author == otherBook.author && this.title == otherBook.title)
                    return 0;

                if (this.title == otherBook.title)                          //now for this we check the title duplicates
                    return string.Compare(this.author, otherBook.author);   //and then compare authors if true.

                return string.Compare(this.title, otherBook.title);

                
            }
        }
        Book tFront;      //initialize front references 
        Book aFront;      //for author and title lists
                         

        public ErrorCode Add(string author, string title, double price)
        {
            Book nB = new Book(author, title, price);       // no matter what you want to make the new object 

            bool auTest = false;    //set up 2 bool tests to return true to break out of 
            bool tiTest = false;    //list traversal for each list

            if (aFront == null)     //first we want to add a node if the author list and title list
            {                       //if the author front is null.
                aFront = nB;
                tFront = nB;
                return ErrorCode.OK;
            }
           
            int checkA = nB.CompareByAuthor(aFront);    //since the CompareBy methods return integers
            int checkT = nB.CompareByTitle(tFront);    //it's good practice to set it instead of what
                                                        //I did which checked this in each if statement

            //We want to see if the new book will go in front of, or follow the front of both aFront and tFront.
            //Of course we need to also check if the books are duplicates and if the next item in the list is null
            //if so we add a new one and we are done, so we make the auTest and tiTest true to avoid the while loops
            //so we just return OK
            
            if (checkA == -1)
            {
                nB.aNext =aFront;
                aFront = nB;
                auTest = true;
            }
            if(checkT==-1)
            {
                nB.tNext = tFront;
                tFront = nB;
                tiTest = true;
            }
            if (checkA == 0)
            {
                auTest = true;
                return ErrorCode.DuplicateBook;
            }
            if (checkT == 0)
            {
                tiTest = true;
                return ErrorCode.DuplicateBook;
            }

            if (aFront.aNext == null)
            {
                aFront.aNext = nB;
                auTest = true;
            }
            if (tFront.tNext == null)
            {
                tFront.tNext = nB;
                tiTest = true;
            }
           if (auTest == true && tiTest == true)
                return ErrorCode.OK;
      

            //So we are going to crawl through the list while cur.aNext is not null
            //and the test is false, we want to make another comparative int to compare
            //the book to cur.aNext (then further down the list if we need to) first we check
            //for duplicates then we check if the book goes after cur.aNext if so and the aNext.aNext
            //is null then add the new book here and break out of the loop, if it goes before then we 
            //do a basic node swap. If none of that can happen then we traverse the list, this will be
            //the same for the title test then we return the errorCode.OK

            Book cur = aFront;
            while (auTest == false && cur.aNext != null) 
            {
                int compInt = nB.CompareByAuthor(cur.aNext);

                if (compInt == 0)
                    return ErrorCode.DuplicateBook;
                if (compInt == 1)
                {
                    if (cur.aNext.aNext == null)
                    {
                        cur.aNext.aNext = nB;
                        auTest = true;
                    }
                }
                if (compInt == -1)
                {
                    nB.aNext = cur.aNext;
                    cur.aNext = nB;
                    auTest = true;
                    
                }
                cur = cur.aNext;
            }

            cur = tFront;
            while (tiTest == false && cur.tNext != null)
            {

                int compInt2 = nB.CompareByTitle(cur.tNext);

                if (compInt2 == 0)
                    return ErrorCode.DuplicateBook;

                if (compInt2 == 1)
                {
                    if (cur.tNext.tNext == null)
                    {
                        cur.tNext.tNext = nB;
                        tiTest = true;
                    }
                 
                }
                if (compInt2 == -1)
                {
                    nB.tNext = cur.tNext;
                    cur.tNext = nB;
                    tiTest = true;
                  
                }
                cur = cur.tNext;
            }
        
            
            return ErrorCode.OK;

        }
        //In the printby mehod we check if front(temp) is null
        //if not then give error message
        //then while temp is not null we print the data
        //using the print function we made in Book class
        //and traverse the lists.

        public void PrintByAuthor()
        {
            Book temp = aFront;
            if(temp == null)
                Console.WriteLine("Null aList");

            while (temp != null)
            {
                temp.Print(temp);
                temp = temp.aNext;
            }
        }
        public void PrintByTitle()
        {
            Book temp = tFront;
            if (temp == null)
                Console.WriteLine("Null tList");
            while (temp != null)
            {
                temp.Print(temp);
                temp = temp.tNext;
            }
        }
        //In remove I used a similar method of removing from the list as I did by adding
        //2 booleans to check the author list and title list. Start by checking if fron is null
        //then return an error code. Next we want to check if the the first node in each list
        //to see if it equals the string passed in. If so it should be found in both lists then return true.
        //Then we want to skip the while loops (if possible) so if found in A/T list is true then return
        //If not we crawl through the lists and check for the data match, if so then cur.next = cur.next.next
        //to cut the node. Then we check if both tests are true again, then return ok if not then the only
        //case left is to be not in the list(s). 
        
        public ErrorCode Remove(string aut, string titl)
        {
            bool foundInAList = false;
            bool foundInTList = false;

            if (aFront == null)                 //if the authorFront is null then the list is empty
                return ErrorCode.BookNotFound;

            if(aFront.author == aut && aFront.title == titl)
            {
                aFront = aFront.aNext;
                foundInAList = true;
            }
            if (tFront.author == aut && tFront.title == titl)
            {
                tFront = tFront.tNext;
                foundInTList = true;
            }
            if (foundInAList == true && foundInTList == true)
                return ErrorCode.OK;

            Book cur = aFront;

            while (foundInAList == false && cur.aNext != null)
            {
                if (cur.aNext.author == aut && cur.aNext.title == titl)
                {
                    cur.aNext = cur.aNext.aNext;
                    foundInAList = true;
                }
                cur = cur.aNext;
            }

            cur = tFront;
            while ( foundInTList == false && cur.tNext != null)
            {
                if (cur.tNext.author == aut && cur.tNext.title == titl)
                {
                    cur.tNext = cur.tNext.tNext;
                    foundInTList = true;
                }
                cur = cur.tNext;

            }
            if (foundInAList == true && foundInTList == true)
                return ErrorCode.OK;

            return ErrorCode.BookNotFound;
        }
    }
}
