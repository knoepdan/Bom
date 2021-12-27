import * as React from 'react';
import c from 'style/cssClasses';
import css from './HamburgerMenu.module.css';
import { SideNavHtmlId } from './SideNav';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

// more or less random notes about webpack setup
export const HamburgerMenu = (): React.ReactElement<Props> => {
    const handleClick = (): void => {
        const sideNav = document.getElementById(SideNavHtmlId);
        if (sideNav) {
            if (sideNav.style.display === 'block') {
                console.log('open sidenave -> set to none');
                sideNav.style.display = 'none';
            } else {
                console.log('open sidenave -> set to block');
                sideNav.style.display = 'block';
            }
        }
    };

    // TODO -> Add hideLarge class !!!! <button className="hambButton hideLarge">
    return (
        <button className={css.hambButton + ' ' + c('tabletAndSmaller')}>
            <div>
                <input type="checkbox" id="hamburg" className={css.hambCheckbox} />
                <label htmlFor="hamburg" className={css.hamburg} onClick={handleClick}>
                    <span className={css.hambLine}></span>
                    <span className={css.hambLine}></span>
                    <span className={css.hambLine}></span>
                </label>
            </div>
        </button>
    );
};

export default HamburgerMenu as React.FC;
