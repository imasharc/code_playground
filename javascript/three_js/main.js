import * as THREE from "three";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls";
import * as dat from "dat.gui";

// tool that three.js uses to allocate a space on the webpage where we can further add and animate all objects
const renderer = new THREE.WebGLRenderer();
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);

const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(
  45,
  window.innerWidth / window.innerHeight,
  0.1,
  1000
);

// OrbitControls allows to add mobility to camera in order to see elements on the screen from different angles using mouse buttons
const orbit = new OrbitControls(camera, renderer.domElement);

// tool that servers as a guide by introducing 3D coordinate system
const axesHelper = new THREE.AxesHelper(3);
scene.add(axesHelper);

// adding plane
const planeGeometry = new THREE.PlaneGeometry(30, 30);
const planeMaterial = new THREE.MeshBasicMaterial({
  color: 0x99bbff,
  side: THREE.DoubleSide,
});
const plane = new THREE.Mesh(planeGeometry, planeMaterial);
scene.add(plane);
plane.rotation.x = -0.5 * Math.PI;

// add grid helper
const gridHelper = new THREE.GridHelper(30);
scene.add(gridHelper);

// add sphere
const sphereGeometry = new THREE.SphereGeometry(4, 50, 50);
const sphereMaterial = new THREE.MeshBasicMaterial({
  color: 0x0000ff,
  wireframe: true,
});
const sphere = new THREE.Mesh(sphereGeometry, sphereMaterial);
scene.add(sphere);

sphere.position.set(-10, 10, 0);

// changing camera position in order to see the helper axes
camera.position.set(-20, 20, 30);
orbit.update();

// adding box
const boxGeometry = new THREE.BoxGeometry();
const boxMaterial = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
const box = new THREE.Mesh(boxGeometry, boxMaterial);
scene.add(box);

// dat.GUI controls
const gui = new dat.GUI();
const options = {
  sphereColor: "#ffea00",
  wireframe: true,
  speed: 0.01,
};
// options to change the color of the sphere
gui.addColor(options, "sphereColor").onChange(function (e) {
  sphere.material.color.set(e);
});
// options to change the display of the wireframe of the sphere
gui.add(options, "wireframe").onChange(function (e) {
  sphere.material.wireframe = e;
});
gui.add(options, "speed", 0.01);

// attributes for making the sphere/ball bounce
let step = 0;

// continuously update the rotation of the box object based on the current time (timestamp)
// By dividing the time by 1000, it converts the time from milliseconds to seconds
// The resulting value is then assigned to the rotation.y and rotation.x properties of the box, causing it to rotate over time.
function animate(time) {
  console.log(time);

  // rotating the box
  box.rotation.y = time / 1000;
  box.rotation.x = time / 1000;

  // bouncing the sphere/ball
  step += options.speed;
  sphere.position.y = 10 * Math.abs(Math.sin(step));

  renderer.render(scene, camera);
}

renderer.setAnimationLoop(animate);
