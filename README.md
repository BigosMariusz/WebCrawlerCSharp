# WebCrawlerCSharp
### Description
Asynchronous web crawler with parallel elements written in C#. Crawler goes online and collects all internal and external links from websites. Links are saved in the MySQL DB. 

### Key problem
The key problem was how to optimize time which crawler spend on walking on websites.  First thing may be to use an async method and than waiting for all the tasks to be done but if in tier i have 1 million sites it isn't the best for my RAM. So my solution was to divide list of links which i need visit into packs of 100 links. It help not overload my RAM but performance is still  2 times better compared with synchronous working. 

### First startup
Chose 2 option:

![imgur image](https://i.imgur.com/FIuu8Ej.png)

Pass your DB conection details:

![imgur image](https://i.imgur.com/RDSi7Rq.png)

Chose 1 option(DB conncetion is checking):

![imgur image](https://i.imgur.com/FIuu8Ej.png)

Pass start url, hop size (1 mean that crawler will visit only on start url), and delay (I recommend 0):

![imgur image](https://i.imgur.com/KgXq7WW.png)

Searching is started...
When screen will be like below search is ended. Statistic are given, if you want go to main menu press enter:

![imgur image](https://i.imgur.com/X9mRXPC.png)

In DB you have results:

![imgur image](https://i.imgur.com/Zj0GWqJ.png)

Warning - if you start new search, links from your DB will be overwritten.

### Requirements
To run this program you need MySQL server on your computer.

### DB schema
Id - Link id

Url - Link url

ParentId - Id of the link from which I came to this page

Domain - Domain of link

IsInternal - Is the link within 1 domain with the parent

Tier - Level of link, start level is 0
