var GameTest = require('../models/GameTest')


module.exports.getAllGameData = function(req,res){
  var sort = {"myScreenName" : 1}
    GameTest.find().sort(sort).then(function(gamedata){
      console.log(gamedata)
      res.json(gamedata)
    })
}

module.exports.getOneByName = function(req,res){
  console.log("Selected by myScreenName!", req.query.myScreenName);
  GameTest.findOne({"myScreenName":req.query.myScreenName}).then(function(gamedata){
    console.log(gamedata)
    res.json(gamedata)
  })
}

module.exports.deleteEntry = function(req, res){
  console.log("Screen Name to delete: ", req.query.myScreenName);
  GameTest.deleteOne({"myScreenName":req.query.myScreenName}).then(function(){
    console.log("Entry deleted")
  }).catch(function(err){
    console.log(err)
  })
}

module.exports.updateEntry = function(req, res){
  console.log("Screen name to update: ", req.body.myScreenName);
  
  GameTest.updateOne(
    {myScreenName: req.body.myScreenName},
    {myScreenName: req.body.myScreenName,
    myFirstName:req.body.myFirstName,
    myLastName:req.body.myLastName,
    myDate:req.body.myDate,
    myScore:req.body.myScore}
  ).catch(function(err){
    console.log(err)
  })
}

module.exports.saveEntry = function(req,res){
  var errors = []
  
  if(req.body.myScreenName == ""){
    errors.push({text:"Screen name Not added"})
  }
  if(req.body.myFirstName == ""){
    errors.push({text:"First name Not added"})
  }
  if(req.body.myLastName == ""){
    errors.push({text:"Last name Not added"})
  }
  if(req.body.myScore == 0){
    errors.push({text:"No Score Added"})
  }
  if(req.body.myDate == ""){
    errors.push({text:"Date Not added"})
  }
  //if there are errors don't validate if there aren't log and save data
  if(errors.length > 0){
    console.log({
      errors:errors
    })
  }else{
    console.log("Hello from Unity Post ", req.body)
    var gameTestData = {
      myScreenName:req.body.myScreenName,
      myFirstName:req.body.myFirstName,
      myLastName:req.body.myLastName,
      myDate:req.body.myDate,
      myScore:req.body.myScore
    }
    console.log(gameTestData)
    GameTest(gameTestData).save().then(function(data){
      console.log("Data Saved")
    }).catch(function(err){
      console.log(err)
    })
  } 
}