# Blockchain
System Integration Blockchain

## Teknisk Dokument
Vi har udviklet en prototype for BlockChain og implementeret en række funktioner f.eks `mining of blocks`, men har stadig nogle få bugs som vi ikke har tid at fikse. Programmet kører på P2P netværk og har 4 clients.

### Setup
For at kunne køre Programmet, skal man pre-install docker og docker skal køres i baggrund. <br/>
**Get Docker**: https://docs.docker.com/

### Deploy
For at kunne køre Programmet korrekt, skal man ifølge instruktion:
1. Starter Terminal for macOS/Linux eller Command Prompt for Windows i den rigtig mappe, dvs. i projekt mappen hvor 
`docker-compose.yml` fil er.
2. Skriver `docker-compose up --build` i terminal eller command prompt.
3. Venter til alle container er hentet og installation er færdig. <br/>

**Bemærk**, programmet vil køre automatisk efter installationen og mens programmet kører, vil der kaste nogle errors eller exceptions.


## Sources
Vi har ved hjælpe af følge links, programmeret denne prototype. <br/>
https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/ <br/>
https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-p2p-network/ <br/>
https://docs.docker.com/network/ <br/>
https://docs.docker.com/compose/networking/ <br/>
https://hub.docker.com/r/microsoft/dotnet/ <br/>
https://stackoverflow.com/ <br/>
