class Vector2
{
   constructor(x, y) {
      this.x = x;
      this.y = y;
   }

   add(right) {
      return new Vector2(this.x + right.x,
                         this.y + right.y);
   }

   addself(right) {
      this.x += right.x;
      this.y += right.y;
   }

   reset() {
      this.x = 0.0;
      this.y = 0.0;
   }


}

module.exports = Vector2;