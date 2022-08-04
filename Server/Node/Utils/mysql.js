const mysql2 = require("mysql2/promise");
require("dotenv").config();

class mysql
{
   constructor() {
      this.con;
   }

   async init() {
      this.con = await mysql2.createConnection({
         host: process.env.DB_HOST,
         user: process.env.DB_USER,
         password: process.env.DB_PASSWORD,
         database: process.env.DB_DATABASE
      });

      return 1;
   }

   async query(sql, data) {
      const [rows, fields] = await this.con.query(sql, data);
      return rows;
   }
}

module.exports = new mysql();