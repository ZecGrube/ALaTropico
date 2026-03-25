# 3D Assets Specification for «Caudillo Bay»

This document provides a comprehensive list of required 3D models for the project, categorized by functional group, including technical specifications for the art team.

## General Technical Requirements
*   **Art Style:** Vibrant, "Caribbean" semi-realism (similar to Tropico 6 or Anno 1800).
*   **PBR Workflow:** Albedo, Normal, Metallic/Smoothness, Ambient Occlusion.
*   **Textures:** Atlas-based for small props, 2048x2048 for major buildings.
*   **LODs:** Minimum 3 levels (LOD0: Detail, LOD1: Mid, LOD2: Silhouette).
*   **Grid Alignment:** All buildings must snap to the 1x1 meter grid (Footprint).
*   **Polygon Budget:**
    *   Small Buildings (1x1): 1,500–3,000 tris.
    *   Medium Buildings (3x3): 5,000–8,000 tris.
    *   Unique Landmarks: Up to 15,000 tris.
    *   Characters: 2,000–4,000 tris.

---

## 1. Residential Buildings
| Asset Name | Description | Footprint | Visual Notes |
| :--- | :--- | :--- | :--- |
| **Chabola (Shack)** | Slum housing made of corrugated metal and scrap wood. | 1x1 | Dirty textures, laundry lines. |
| **Casa de Madera** | Colonial-style wooden house. | 2x2 | Bright paint (Pink/Teal), porch. |
| **Bloque de Apartamentos** | "Khrushchyovka" style tropical apartment block. | 3x4 | AC units, satellite dishes on facade. |
| **Villa de Lujo** | Luxury mansion with a private pool. | 4x4 | White stone, yard palms, gated. |

## 2. Agriculture & Resources
| Asset Name | Description | Footprint | Visual Notes |
| :--- | :--- | :--- | :--- |
| **Banana Plantation** | Rows of banana trees with a small crate shed. | 4x4 | Animated palm leaves. |
| **Sugar Mill** | Industrial mill with a smoking chimney. | 3x3 | Rotating gears/shafts visible. |
| **Uranium Mine** | Open-pit or shaft mine for uranium ore. | 5x5 | Trucks, radiation signs, yellow dust. |
| **Sawmill** | Wooden structure for processing logs. | 3x2 | Circular saw sounds, wood piles. |
| **Enrichment Plant** | Facility for processing uranium ore into fuel. | 4x4 | Centrifuge tubes, gas pipes, heavy security. |
| **Offshore Platform** | Deep-sea oil/gas drilling rig. | 6x6 | Water-based. Flame on flare stack. |

## 3. Industry & Utilities
| Asset Name | Description | Footprint | Visual Notes |
| :--- | :--- | :--- | :--- |
| **Rum Distillery** | Factory with large copper fermentation tanks. | 3x3 | Glass bottle conveyor belts (visual). |
| **Coal Power Plant** | Old-fashioned power plant with chimneys. | 4x4 | Thick black smoke, coal piles. |
| **Nuclear Plant** | Dual cooling tower atomic station. | 6x6 | Steam effect from towers. Modern concrete. |
| **Water Pump** | Station on river or coast for water extraction. | 2x2 | Pipes into water, humming sound. |
| **Robot Factory** | High-tech assembly plant with robotic arms. | 4x4 | Clean sci-fi style, neon accents. |
| **Automated Farm** | High-tech hydroponic vertical farm. | 4x4 | Glass facade, purple glow, solar roof. |
| **Warehouse** | Large storage facility for various goods. | 4x4 | Cargo doors, loading docks. |
| **Water Treatment** | Potabilization plant with circular tanks. | 3x3 | Glossy water surfaces. |

## 4. Government & Landmarks
| Asset Name | Description | Footprint | Visual Notes |
| :--- | :--- | :--- | :--- |
| **Presidential Palace** | Grand Neoclassical government seat. | 8x6 | Flag on roof, speech balcony. |
| **Statue of El Presidente** | Golden full-body statue of the leader. | 2x2 | Heroic pose, granite pedestal. |
| **Espionage Center** | Discreet building with a massive antenna array. | 3x3 | Rotating radar on roof. |
| **Summit Hall** | Modern glass-and-steel diplomatic complex. | 6x4 | Flags of many nations, VIP helipad. |
| **Police Station** | Local security office with a few jail cells. | 2x2 | Blue light, parked patrol car. |
| **Catholic Church** | Traditional colonial-style church. | 3x4 | Bell tower, stained glass. |
| **Hospital** | Modern medical facility. | 4x4 | Helipad on roof, ambulance bay. |
| **School/University** | Educational institution. | 4x6 | Clock tower, courtyard. |
| **Military Barracks** | Housing and training for soldiers. | 4x4 | Training yard, camouflage netting. |
| **Missile Silo** | Underground ICBM launch site. | 3x3 | Opening hatch (animated). |

## 5. Space Program
| Asset Name | Description | Footprint | Visual Notes |
| :--- | :--- | :--- | :--- |
| **Spaceport** | Launch pad with a heavy-lift rocket. | 10x10 | Rocket is a separate mesh for launch. |
| **Space Tourism Center** | Luxury terminal for suborbital flights. | 4x4 | Sleek curved architecture, VIP lounge. |
| **Lunar Module** | Landing craft for the Moon. | 2x2 | Gold foil, landing struts, science gear. |
| **Satellites (6 Types)** | Comm, Nav, Recon, Sci, Weather, Mil. | - | Small models for orbital view. |

## 6. Vehicles & Agents
| Asset Name | Type | Specs | Visual Notes |
| :--- | :--- | :--- | :--- |
| **Logistics Truck** | Vehicle | Low-Poly | 1950s style vintage truck, colorful. |
| **Delivery Drone** | Vehicle | Sci-Fi | Quadcopter with a cargo container. |
| **El Presidente** | Character | Animated | Epaulettes, beard, cigar. |
| **Android (Synth)** | Character | Animated | Humanoid with transparent hull parts. |
| **Patria Tank** | Unit | Animated | T-55 style soviet tank, palm camo. |
| **Research Vessel** | Vehicle | Water | Science masts, crane on deck, white hull. |
| **Oil Tanker** | Vehicle | Water | Massive cargo hull, red/black paint. |

## 7. Environment Tiles
*   **Soil Types:** Sand, Grass, Forest (dense foliage), Mountains (rocky).
*   **Foliage:** Coconut Palms, Fan Palms, Hibiscus bushes, Sugar Cane stalks.
*   **Props:** Street lights (Vintage/Modern), Benches, Propaganda posters on poles.

---

## Animation Requirements
1.  **Smoke/Steam** for all industrial chimneys (VFX/Particle).
2.  **Rotation** for radars, windmills, and gear assemblies.
3.  **Loading/Unloading** poses for logistics docks.
4.  **Waving Flags** (Palace, Military bases).
5.  **Rocket Launch** sequence.
