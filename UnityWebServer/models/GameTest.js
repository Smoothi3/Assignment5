var mongoose = require("mongoose")
var Schema = mongoose.Schema;

var GameTestSchema = new Schema({
    myScreenName:{
        type:String,
        required:true
    },
    myFirstName:{
        type:String,
    },
    myLastName:{
        type:String,
    },
    myDate:{
        type:String,
    },
    myScore:{
        type:Number
    }
})

let GameTest = mongoose.model('gameTest', GameTestSchema)
module.exports = GameTest