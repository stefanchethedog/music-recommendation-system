import { FC } from "react";
import Button from "@mui/material/Button";

import { Navbar, Song, SongList } from "../../components";

import "./LandingPage.styles.scss";
import { SongProps } from "../../components/Song/Song.component";

const songs: SongProps[] =[
  {name: 'Zal', author:'Saban', genres:['Narodnjak']},
  {name: 'What is love', author:'Marko Jack', genres:['Punk']},
  {name: 'Unstopable', author: 'Sia', genres:['Pop']},
  {name: 'Redrum', author:'21 Savage', genres:['Rap','Hip hop', "Trippity dop"]},
  {name: 'Zal', author:'Saban', genres:['Narodnjak']},
  {name: 'What is love', author:'Marko Jack', genres:['Punk']},
  {name: 'Unstopable', author: 'Sia', genres:['Pop']},
  {name: 'Redrum', author:'21 Savage', genres:['Rap','Hip hop', "Trippity dop"]},
  {name: 'Zal', author:'Saban', genres:['Narodnjak']},
  {name: 'What is love', author:'Marko Jack', genres:['Punk']},
  {name: 'Unstopable', author: 'Sia', genres:['Pop']},
  {name: 'Redrum', author:'21 Savage', genres:['Rap','Hip hop', "Trippity dop"]},
]

const LandingPage: FC = () => {
  return (
    <div className="landing-page__container">
      <Navbar
        className="landing-page__container__navbar"
        listItems={[{ label: "Username", value: "Stefanche" }]}
      />
      <div className="landing-page__container__body">
        <SongList title="Recommended songs" songData={songs}/>
      </div>
    </div>
  );
};

export default LandingPage;
