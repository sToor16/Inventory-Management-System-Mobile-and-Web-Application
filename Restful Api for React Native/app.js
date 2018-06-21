var sql = require('mssql');
const express=require('express');
const app=express();
const bodyParser=require("body-parser");
app.use(bodyParser.json({limit:'50mb'}));

var sqlConfig = {
  user: 'Toor',
  password: '************',
  server: '******************',  
  database: '*************'
};

app.get('/',function(req,res){
    res.send({error:false,message:'check fromm lambda'});
});

app.post('/getPassword',function(req,res){
      (async function () {
      try {
        let pool = await sql.connect(sqlConfig)
		    let USERNAME = req.body.USERNAME;
		    let PASSWORD = req.body.PASSWORD;
        let result = await pool.request()
          .query(`select Id from Employees 
              WHERE UserName = '`+ USERNAME+`' 
              AND Password = '`+ PASSWORD+`'
              AND IsAdmin != 1
              AND IsActive != 0 `)
    
        var fetchId=JSON.parse(JSON.stringify(result));
        		res.send({
        			error:false,status:800,message:"done",fetchId:fetchId
        			
          });
          sql.close();
      } catch (err) {
        console.log(err);
        sql.close();
      }
    })()
});

app.post('/getFacilities',function(req,res){
      (async function () {
      try {
        let pool = await sql.connect(sqlConfig)
		let Employee_Id = req.body.Employee_Id;
        let result = await pool.request()
          .query(`select FacilityEmployees.Facility_FacilityId, Facilities.FacilityName, Facilities.Description, Facilities.Landmark,
					Facilities.Address, Facilities.City, Facilities.State, Facilities.ZipCode  from FacilityEmployees 
				INNER JOIN Facilities ON Facilities.FacilityId = FacilityEmployees.Facility_FacilityId 
				WHERE Employee_Id = '`+ Employee_Id+`'
				AND IsActive != 0`, function(err, recordset){
            if (err) console.log(err)
            res.send(recordset);
            sql.close();
          })
      } catch (err) {
        console.log(err);
        sql.close();
      }
    })()
});

app.post('/getResources',function(req,res){
      (async function () {
      try {
        let pool = await sql.connect(sqlConfig)
		let FacilityId = req.body.FacilityId;
        let result = await pool.request()
          .query(`select * from Resources 
          WHERE FacilityId = '`+ FacilityId+`'
          AND IsActive != 0`, function(err, recordset){
            if (err) console.log(err)
            res.send(recordset);
			sql.close();
          })
      } catch (err) {
        console.log(err);
		sql.close();
      }
    })()
});

app.post('/getSelectedResources',function(req,res){
      (async function () {
      try {
        let pool = await sql.connect(sqlConfig)
		let ResourceId = req.body.ResourceId;
        let result = await pool.request()
          .query(`select * from Resources WHERE ResourceId = '`+ ResourceId+`'`, function(err, recordset){
            if (err) console.log(err)
            res.send(recordset);
			sql.close();
          })
      } catch (err) {
        console.log(err);
		sql.close();
      }
    })()
});

app.put('/editSelectedResource',function(req,res){
      (async function () {
      try {
        let pool = await sql.connect(sqlConfig)
		let ResourceId = req.body.ResourceId;
		let ResourceName = req.body.ResourceName;
		let Quantity = req.body.Quantity;
		let Description = req.body.Description;
		let Size = req.body.Size;
		let Color = req.body.Color;
		let FacilityId = req.body.FacilityId;
		let PreviousValue = req.body.PreviousValue;
        let result = await pool.request()
          .query(`UPDATE Resources SET Quantity = '`+ Quantity+`', 
              Description = '`+Description+`', Size = '`+Size+`', Color = '`+Color+`' 
              where ResourceId = '`+ ResourceId+`'`, function(err, recordset){
            if (err) console.log(err)
            res.send(recordset);
			sql.close();
          })
      } catch (err) {
        console.log(err);
		sql.close();
      }
    })()
});

module.exports = app;

//app.listen(process.env.PORT||8080, function(){
//    console.log("Listening on 5000 port");
//});