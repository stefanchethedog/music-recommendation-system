import { Dispatch, FC, SetStateAction, useState } from "react";
import classNames from "classnames";
import UserIcon from "@mui/icons-material/Person";
import ArtistIcon from "@mui/icons-material/RecordVoiceOver";
import SongsIcon from "@mui/icons-material/LibraryMusic";
import AlbumIcon from "@mui/icons-material/Album";
import HeadphonesIcon from "@mui/icons-material/Headphones";

import {
  UserForm,
  AlbumsForm,
  ArtistForm,
  GenresForm,
  SongsForm,
} from "../Forms";

import "./Navbar.styles.scss";

type NavbarItem = {
  label: String;
  value: String;
};

type NavbarProps = {
  selected: number,
  setSelected: Dispatch<SetStateAction<number>>,
  id: String | null;
  listItems: NavbarItem[];
  className?: String;
};

const sidebarObjects = [
  {
    title: "User",
    icon: <UserIcon />,
  },
  {
    title: "Artist",
    icon: <ArtistIcon />,
  },
  {
    title: "Songs",
    icon: <SongsIcon />,
  },
  {
    title: "Albums",
    icon: <AlbumIcon />,
  },
  {
    title: "Genres",
    icon: <HeadphonesIcon />,
  },
];

const Navbar: FC<NavbarProps> = ({ className, selected, setSelected }) => {
  const classes = classNames("navbar__container", className);


  const generateBody = () => {
    if (selected === 0) return <UserForm />;
    if (selected === 1) return <ArtistForm />;
    if (selected === 2) return <SongsForm />;
    if (selected === 3) return <AlbumsForm />;
    if (selected === 4) return <GenresForm />;
  };

  return (
    <div className="navbar">
      <div className="navbar__container">
        <div className="navbar__container__sidebar">
          {sidebarObjects.map((current, index) => (
            <div
              className={`navbar__container__sidebar__item navbar__container__sidebar__item${
                index === selected ? "--selected" : ""
              }`}
              onClick={() => {
                setSelected(index);
              }}
              id={`navbar__sidebar${index}`}
            >
              {current.icon}
              <small className="navbar__container__sidebar__item__description">
                {current.title}
              </small>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Navbar;
