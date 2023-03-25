# Console Application for Indexing Ethereum Blockchain

This console application is designed to index the blocks and transaction information of the Ethereum blockchain. It starts from Block #12100001 and goes up to Block #12100500. The indexing sequence is as follows:

1. Get the desired block with the method eth_getBlockByNumber. The block number needs to be converted to hexadecimal format (Ex. 0xB8A1A1).
2. If the block is found, call eth_getBlockTransactionCountByNumber to get the count of transactions in the block. Insert the block record into the database.
3. If the transaction count is not zero, call eth_getTransactionByBlockNumberAndIndex to retrieve the transaction information line by line. Insert the record into the database.
4. The entire process should be logged accordingly to a text file, and a timestamp and processing time should be logged in both the console and the logfile.

## Installation

1. Install the .NET Framework if it is not already installed.
2. Install the MySQL Connector/NET if it is not already installed.
3. Clone the repository or download the source code.
4. Open the solution in Visual Studio.
5. Build the solution.

## Configuration
- Open the appsettings.json file and set the connection string for the MySQL database and API key.
