import * as THREE from "three";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls";
import * as dat from "dat.gui";

// tool that three.js uses to allocate a space on the webpage where we can further add and animate all objects
const renderer = new THREE.WebGLRenderer();
// enable shadowMap in order to add shadows
renderer.shadowMap.enabled = true;
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
const planeMaterial = new THREE.MeshStandardMaterial({
  color: 0xffffff,
  side: THREE.DoubleSide,
});
const plane = new THREE.Mesh(planeGeometry, planeMaterial);
scene.add(plane);
plane.rotation.x = -0.5 * Math.PI;
// plane receives the shadows emitted by the sphere being in front of the light source
plane.receiveShadow = true;

// add grid helper
const gridHelper = new THREE.GridHelper(30);
scene.add(gridHelper);

// add sphere
const sphereGeometry = new THREE.SphereGeometry(4, 50, 50);
const sphereMaterial = new THREE.MeshStandardMaterial({
  color: 0x0000ff,
  wireframe: false,
});
const sphere = new THREE.Mesh(sphereGeometry, sphereMaterial);
scene.add(sphere);
sphere.position.set(-10, 10, 0);
// sphere casts the shadows by being in front of the light source
sphere.castShadow = true;

// adding lights
const ambientLight = new THREE.AmbientLight(0x333333);
scene.add(ambientLight);

// const directionalLight = new THREE.DirectionalLight(0xffffff);
// scene.add(directionalLight);
// directionalLight.position.set(-20, 30, 0);
// // directional light casts the shadows by being the main source of the shadows creation
// directionalLight.castShadow = true;
// directionalLight.shadow.camera.bottom = -12;

// const dLightHelper = new THREE.DirectionalLightHelper(directionalLight, 3);
// scene.add(dLightHelper);

// const dLightShadowHelper = new THREE.CameraHelper(
//   directionalLight.shadow.camera
// );
// scene.add(dLightShadowHelper);

// add spotlight
const spotLight = new THREE.SpotLight(0xffffff);
scene.add(spotLight);
spotLight.position.set(-50, 50, 0);
spotLight.castShadow = true;
spotLight.angle = 0.2;
// add spotlight helper
const sLightHelper = new THREE.SpotLightHelper(spotLight);
scene.add(sLightHelper);

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
  wireframe: false,
  speed: 0.01,
  angle: 0.2,
  penumbra: 0,
  intensity: 1,
};
// options to change the color of the sphere
gui.addColor(options, "sphereColor").onChange(function (e) {
  sphere.material.color.set(e);
});
// options to change the display of the wireframe of the sphere
gui.add(options, "wireframe").onChange(function (e) {
  sphere.material.wireframe = e;
});
gui.add(options, "speed", 0, 0.1);
gui.add(options, "angle", 0, 1);
gui.add(options, "penumbra", 0, 1);
gui.add(options, "intensity", 0, 1);

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

  // spotlight modification
  spotLight.angle = options.angle;
  spotLight.penumbra = options.penumbra;
  spotLight.intensity = options.intensity;
  sLightHelper.update();

  renderer.render(scene, camera);
}

renderer.setAnimationLoop(animate);
