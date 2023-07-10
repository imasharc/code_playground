﻿import * as THREE from "three";

// tool that three.js uses to allocate a space on the webpage where we can further add and animate all objects
const renderer = new THREE.WebGLRenderer();
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);

const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(
  75,
  window.innerWidth / window.innerHeight,
  0.1,
  1000
);

// tool that servers as a guide by introducing 3D coordinate system
const axesHelper = new THREE.AxesHelper(3);
scene.add(axesHelper);

// changing camera position in order to see the helper axes
camera.position.set(1, 1, 5);

// adding box
const boxGeometry = new THREE.BoxGeometry();
const boxMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
const box = new THREE.Mesh(boxGeometry, boxMaterial);
scene.add(box);

// continuously update the rotation of the box object based on the current time (timestamp)
// By dividing the time by 1000, it converts the time from milliseconds to seconds
// The resulting value is then assigned to the rotation.y and rotation.x properties of the box, causing it to rotate over time.
function animate(time) {
  console.log(time);
  box.rotation.y = time / 1000;
  box.rotation.x = time / 1000;
  renderer.render(scene, camera);
}

renderer.setAnimationLoop(animate);
