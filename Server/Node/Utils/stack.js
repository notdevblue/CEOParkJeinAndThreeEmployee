class Stack {
   constructor() {
      this.arr = [];
   }

   push(item) {
      this.arr.push(item);
   }

   pop() {
      return this.arr.pop();
   }

   peek() {
      return this.arr[this.arr.length - 1];
   }
}

module.exports = Stack;