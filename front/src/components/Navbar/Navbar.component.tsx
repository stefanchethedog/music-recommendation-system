import { FC } from "react";
import classNames from "classnames";

import "./Navbar.styles.scss";

type NavbarItem = {
  label: String;
  value: String;
};

type NavbarProps = {
  className?: String;
  listItems: NavbarItem[];
};

const Navbar: FC<NavbarProps> = ({ className, listItems }) => {
  const classes = classNames("navbar__container", className);
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
    </div>
  );
};

export default Navbar;
