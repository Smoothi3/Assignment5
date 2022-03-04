var express = require('express');
var router = express.Router();
var mongoose = require('mongoose');
var bodyParser = require('body-parser');
const { json } = require('body-parser');
var ctrlGameData = require('./gameDataController')

router.use(bodyParser.json())
router.use(bodyParser.urlencoded({extended:true}))
router.use(express.json())

require("../models/GameTest")
var GameTest = mongoose.model('gameTest')

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'Joe' });
});

router.get('/unity', ctrlGameData.getAllGameData)

router.get('/unityGetOne', ctrlGameData.getOneByName)

router.post('/unityPost', ctrlGameData.saveEntry)

router.post('/unityDeleteEntry', ctrlGameData.deleteEntry)

router.put('/unityUpdate', ctrlGameData.updateEntry)

module.exports = router;