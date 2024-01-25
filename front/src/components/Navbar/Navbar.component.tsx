import { FC, useState } from "react";
import classNames from "classnames";
import { Button, TextField } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import Like from '@mui/icons-material/ThumbsUpDown'
import axios from "axios";
import { FOLLOW_USER, SUBSCRIBE_TO, LIKE_SONG } from "../../endpoints";

import "./Navbar.styles.scss";

type NavbarItem = {
  label: String;
  value: String;
};

type NavbarProps = {
  className?: String;
  listItems: NavbarItem[];
  id: String | null;
};

const Navbar: FC<NavbarProps> = ({ className, listItems, id }) => {
  const classes = classNames("navbar__container", className);

  const [usernameToFollow, setUsernameToFollow] = useState<String>("");
  const [nameOfArtist, setNameOfArtist] = useState<String>("");
  const [nameOfSong, setNameOfSong] = useState<String>("");



  const handleSubmitFollow = async () => {
    if (id)
      await axios
        .post(FOLLOW_USER(id, usernameToFollow))
        .then((res) => {
          console.log(res);
        })
        .catch((err) => {
          console.log(err);
        });
  };

  const handleSubmitSubscribe = async () => {
    if (id)
      await axios
        .post(SUBSCRIBE_TO(id, nameOfArtist))
        .then((res) => {
          console.log(res);
        })
        .catch((err) => {
          console.log(err);
        });
  }

  const handleAddLikedSong = async() =>{
    if (id)
      await axios
        .post(LIKE_SONG(id, nameOfSong))
        .then((res) => {
          console.log(res);
        })
        .catch((err) => {
          console.log(err);
        });
  }

  return (
    <div className={classes}>
      <h1 className="navbar__container__title">SONG RECOMMENDER</h1>
      <div className="navbar__container__list">
        {listItems.map((current, index) => {
          return (
            <div className="navbar__container__list__item">
              {current.label}: {current.value}
            </div>
          );
        })}
      </div>
      <div className="navbar__container__input">
        <div className="navbar__container__input__text-field--wrapper">
          <TextField
            variant="filled"
            className="navbar__container__input__text-field"
            label="Username"
            onChange={(e) => {
              setUsernameToFollow(e.target.value);
              console.log(e.target.value);
            }}
          />
        </div>
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          onClick={handleSubmitFollow}
        >
          Follow
        </Button>
      </div>
      <div className="navbar__container__input">
        <div className="navbar__container__input__text-field--wrapper">
          <TextField
            variant="filled"
            className="navbar__container__input__text-field"
            label="Author"
            onChange={(e)=>{
              setNameOfArtist(e.target.value)
              console.log(e.target.value)
            }}
          />
        </div>
        <Button variant="contained" endIcon={<BookmarkIcon />} onClick={handleSubmitSubscribe}>
          Subscribe
        </Button>
      </div>
      <div className="navbar__container__input">
        <div className="navbar__container__input__text-field--wrapper">
          <TextField
            variant="filled"
            className="navbar__container__input__text-field"
            label="Song"
            onChange={(e)=>{
              setNameOfSong(e.target.value)
              console.log(e.target.value)
            }}
          />
        </div>
        <Button variant="contained" endIcon={<Like />} onClick={handleAddLikedSong}>
          Like
        </Button>
      </div>
    </div>
  );
};

export default Navbar;
